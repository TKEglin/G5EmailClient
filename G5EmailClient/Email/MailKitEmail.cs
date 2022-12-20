using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Diagnostics;
using System.Collections;
// Email handlers
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
// Database
using G5EmailClient.Database;

namespace G5EmailClient.Email
{
    public class MailKitEmail : IEmail
    {
        public class EmailFolder
        {
            public EmailFolder(IMailFolder mainFolder, IMailFolder preloadFolder, string name) 
            { 
                MainImapFolder = mainFolder;
                PreloadImapFolder = preloadFolder;
                FolderName = name; 
            }

            public Mutex Mutex = new();

            public IMailFolder? MainImapFolder    { get; set; }
            public IMailFolder? PreloadImapFolder { get; set; }
            // __Flags__
            public bool isLoaded { get; set; } = false;
            // These bools are used to check and stop a preload if it is progress during a new update
            public bool preloadInProgress { get; set; } = false;
            public bool stopPreload { get; set; } = false;
            // Is used to lock a folder while loading is in progress
            public bool isLocked { get; set; } = false;

            // __Folder Data__
            public string? FolderName;
            public int     FolderIndex;          // Contains the folders index in the emailFolders list
            public int     EnvelopesLoaded  = 0; // Keeps track of how many envelopes are loaded in GUI
            public int     NewMessagesAdded = 0; // New messages added to the end of the UID list need not be loaded
            public List<UniqueId> UIDs { get; set; } = new();
            public Dictionary<UniqueId, MimeMessage>    Messages  { get; set; } = new();
            public Dictionary<UniqueId, bool>           Seen      { get; set; } = new();
            // The following messages only contains envelope information. The remaining fields are empty.
            public Dictionary<UniqueId, IEmail.Message> Envelopes { get; set; } = new();
        }


        IDatabase data = new JSONDatabase();

        /// <summary>
        /// This client is the primary connection used to interact with the imap server
        /// </summary>
        ImapClient mainImapClient = new();
        Mutex MainImapMutex = new();

        /// <summary>
        /// This imap client is used to preload messenges.
        /// </summary>
        ImapClient loadImapClient = new();
        Mutex loadImapMutex = new();

        SmtpClient smtpClient = new();
        Mutex SmtpMutex = new();

        IDatabase.User activeUser = new();

        List<EmailFolder> emailFolders = new();
        EmailFolder? activeFolder;
        EmailFolder? trashFolder = null;
        // This variable is used to throttle preloading speed when multiple folders are loading
        int foldersPreloading = 0;

        // Used to maintain connection with the server
        System.Timers.Timer server_ping_timer = new System.Timers.Timer(30000) { Enabled = true };

        public MailKitEmail()
        {
            // Initalizing class data variables.
            List<IDatabase.User> users = data.GetUsers();

            mainImapClient.Disconnected += MaintainConnection;
            loadImapClient.Disconnected += MaintainConnection;
            smtpClient    .Disconnected += MaintainConnection;
        }

        /// <summary>
        /// This function is called when connection is established and the user is authenticated.
        /// </summary>
        void initialise()
        {
            initializeFolders();
            updateFolder(0); // Updating inbox

            server_ping_timer.Elapsed += MaintainConnection;
        }

         //_________________
        // Utility functions
        #region utility functions

        /// <summary>
        /// Splits and interprets a string of emails. The character ',' is used a separator.
        /// All whitespace in the emails will be ignored.
        /// </summary>
        /// <returns>A list of email addresses. 
        /// Returns null if string is empty or an error in an email is detected.</returns>
        List<string> SplitEmails(string email_string)
        {
            List<string> result = new();
            Debug.WriteLine("String pre split: " + email_string);
            string[] emails = email_string.Split(',');

            foreach(var InputEmail in emails)
            {
                Debug.WriteLine("Email pre replace: " + InputEmail);
                var OutputEmail = InputEmail;
                OutputEmail = InputEmail.Replace(",", "");
                OutputEmail = OutputEmail.Replace(" ", "");
                Debug.WriteLine("Email post replace: " + OutputEmail);
                result.Add(OutputEmail);
            }

            return result;
        }

        #endregion

         //_____________________________________
        // Functions that access the imap server
        #region IMAP server retrieval functions

        /// <summary>
        /// Initializes the folders for the server.
        /// </summary>
        private void initializeFolders()
        {
            // Adding inbox at index 0
            emailFolders.Add(new EmailFolder(mainImapClient.Inbox, 
                                             loadImapClient.Inbox, 
                                             mainImapClient.Inbox.Name));
            string inbox_name = mainImapClient.Inbox.Name;

            // Adding folders
            var mainFolders = mainImapClient.GetFolders(mainImapClient.PersonalNamespaces[0], false);
            var preloadFolders = loadImapClient.GetFolders(loadImapClient.PersonalNamespaces[0], false);
            for (int i = 0; i < mainFolders.Count; i++)
            {
                var mainFolder    = mainFolders[i];
                var preloadFolder = preloadFolders[i];

                // If statement to avoid adding the inbox twice
                if (mainFolder.Name != inbox_name & mainFolder.Exists)
                {
                    var newFolder = new EmailFolder(mainFolder, preloadFolder, mainFolder.Name);

                    emailFolders.Add(newFolder);

                    // Weird way to find the index. Is probably a better way.
                    int index = emailFolders.IndexOf(newFolder);
                    newFolder.FolderIndex = index;

                    if (mainFolder.Attributes.HasFlag(FolderAttributes.Trash))
                    {
                        trashFolder = newFolder;
                    }

                    // Preloading folders
                    // ThreadPool.QueueUserWorkItem(state => updateFolder(index));
                }
            }
        }

        /// <summary>
        /// Sets as active and updates the given folder. This function should be called
        /// everytime a new folder is opened, and intermittently to check for new messages.
        /// </summary>
        /// <param name="folder"></param>
        /// 
        private void updateFolder(int folderIndex)
        {
            var folder = emailFolders[folderIndex];

            MainImapMutex.WaitOne();

            // This will be used to remove duplicates once the new messages have been opened.
            int count = folder.UIDs!.Count;
            // This is necessary to let the user access old messages while inbox is updating.

            folder!.MainImapFolder!.Open(FolderAccess.ReadWrite);

            // Emptying lists and dictionaries to avoid adding things twice on reload
            folder.UIDs.Clear();
            folder.Seen.Clear();
            folder.Envelopes.Clear();

            Debug.WriteLine("Fetching summary items from folder " + folder.FolderName);

            var envelopes = folder.MainImapFolder.Fetch(0, -1,
                                                        MessageSummaryItems.Envelope |
                                                        MessageSummaryItems.UniqueId |
                                                        MessageSummaryItems.Flags);

            envelopes = envelopes.Sort(new List<OrderBy>() { OrderBy.ReverseDate });

            foreach (var items in envelopes)
            {
                folder.UIDs.Add(items.UniqueId);
                folder.Seen.Add(items.UniqueId, items.Flags!.Value.HasFlag(MessageFlags.Seen));

                IEmail.Message envelope = new();
                    envelope.from    = items.Envelope.From.ToString();
                if(items.Envelope.Date != null)
                    envelope.date    = items.Envelope.Date.Value.LocalDateTime.ToString();
                if(items.Envelope.Subject != null)
                    envelope.subject = items.Envelope.Subject.ToString();
                folder.Envelopes.Add(items.UniqueId, envelope);
            }

            folder.isLoaded = true;

            if (FolderUpdateFinished != null)
            {
                this.FolderUpdateFinished(folderIndex, EventArgs.Empty);
            }

            MainImapMutex.ReleaseMutex();

            // Waiting for in-progress preload to stop.
            //while (folder.preloadInProgress)
            //{
            //    folder.stopPreload = true;
            //    Thread.Sleep(50);
            //}
        }

        private void PreloadMessages(EmailFolder folder, List<string> sUIDs)
        {
            foldersPreloading++;
            folder.preloadInProgress = true;

            foreach(var sUID in sUIDs)
            {
                if(folder.stopPreload)
                {
                    Debug.WriteLine("Preload of folder " + folder.FolderName + " interrupted.");
                    folder.preloadInProgress = false;
                    folder.stopPreload = false;
                    return;
                }
                
                var UID = UniqueId.Parse(sUID);
                PreloadMessage(folder, UID);
            }

            Debug.WriteLine("Preload of folder \"" + folder.FolderName + "\" completed.");

            folder.preloadInProgress = false;
            foldersPreloading--;
        }

        void IEmail.PreloadMessages(int folder, List<string> sUIDs)
        {
            ThreadPool.QueueUserWorkItem(state => PreloadMessages(emailFolders[folder], sUIDs));
        }

        private void PreloadMessage(EmailFolder folder, UniqueId UID)
        {
            if (!folder.Messages.ContainsKey(UID))
            {
                loadImapMutex.WaitOne();

                if (!folder!.PreloadImapFolder!.IsOpen)
                    folder!.PreloadImapFolder.Open(FolderAccess.ReadWrite);

                MimeMessage message;

                try
                {
                    message = folder!.PreloadImapFolder!.GetMessage(UID);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("!!!!\nFailed to load message. Error: \n!!!!\n" + ex.ToString());
                    loadImapMutex.ReleaseMutex();
                    return;
                }

                //Debug.WriteLine("Folder " + folder.FolderName + " preloading message with subject " + message.Subject
                //              + ". " + foldersPreloading.ToString() + " folders currently preloading.");

                folder.Messages[UID] = message;

                loadImapMutex.ReleaseMutex();
            }
        }
        void IEmail.PreloadMessage(int folderIndex, string sUID)
        {
            ThreadPool.QueueUserWorkItem(state => PreloadMessage(emailFolders[folderIndex], 
                                                                 UniqueId.Parse(sUID)));
        }

        (List<IEmail.Message> messages, List<string> UIDs) IEmail.SearchFolder(string searchString, IEmail.SearchFlags flags)
        {
            (List<IEmail.Message> messages, List<string> UIDs) envelopes = new();

            if (searchString.Length <= 0 | flags == IEmail.SearchFlags.Empty) return envelopes;

            IList<UniqueId> UIDs = new List<UniqueId>();

            var folder = activeFolder!;

            folder.MainImapFolder!.Open(FolderAccess.ReadWrite);

            SearchQuery Query = SearchQuery.Not(SearchQuery.All);

            if (flags.HasFlag(IEmail.SearchFlags.From))    Query = SearchQuery.Or(Query, 
                                                                   SearchQuery.FromContains(searchString));
            if (flags.HasFlag(IEmail.SearchFlags.Subject)) Query = SearchQuery.Or(Query,
                                                                   SearchQuery.SubjectContains(searchString));
            if (flags.HasFlag(IEmail.SearchFlags.Body))    Query = SearchQuery.Or(Query,
                                                                   SearchQuery.BodyContains(searchString));
            if (flags.HasFlag(IEmail.SearchFlags.Cc))      Query = SearchQuery.Or(Query,
                                                                   SearchQuery.CcContains(searchString));
            if (flags.HasFlag(IEmail.SearchFlags.Bcc))     Query = SearchQuery.Or(Query,
                                                                   SearchQuery.BccContains(searchString));

            foreach (var UID in folder.MainImapFolder.Search(Query))
            {
                if (!UIDs.Contains(UID)) UIDs.Add(UID);
            }

            List<IEmail.Message> messages = new();
            List<string>         stringUIDs = new();
            foreach (var UID in UIDs)
            {
                if (folder.Envelopes.ContainsKey(UID) & folder.Seen.ContainsKey(UID))
                {
                    IEmail.Message message = new();
                    message.subject = folder.Envelopes[UID].subject;
                    message.date = folder.Envelopes[UID].date;
                    message.from = folder.Envelopes[UID].from;
                    message.seen = folder.Seen[UID];
                    messages.Add(message);
                    stringUIDs.Add(UID.ToString());
                }
            }
            envelopes = (messages, stringUIDs);

            return envelopes;
        }

        #endregion

         //__________________________________________
        // Functions that deal with server connection
        #region server connection functions
        void IEmail.Disconnect()
        {
            MainImapMutex.WaitOne();
            mainImapClient.Disconnect(true);

            loadImapMutex.WaitOne();
            loadImapClient.Disconnect(true);

            SmtpMutex.WaitOne();
            smtpClient.Disconnect(true);
        }

        bool IEmail.isConnected()
        {
            return (mainImapClient.IsConnected & smtpClient.IsConnected);
        }

        Exception? IEmail.Connect(string IMAP_hostname, int IMAP_port, string SMTP_hostname, int SMTP_port)
        {
            try
            {
                mainImapClient.Connect(IMAP_hostname, IMAP_port, SecureSocketOptions.SslOnConnect);
                loadImapClient.Connect(IMAP_hostname, IMAP_port, SecureSocketOptions.SslOnConnect);
                smtpClient.Connect(SMTP_hostname, SMTP_port, SecureSocketOptions.SslOnConnect);
            }
            catch(Exception ex)
            {
                ((IEmail)this).Disconnect();
                return ex;
            }
            activeUser.SMTP_hostname = SMTP_hostname;
            activeUser.SMTP_port     = SMTP_port;
            activeUser.IMAP_hostname = IMAP_hostname;
            activeUser.IMAP_port     = IMAP_port;
            return null;
        }

        Exception? IEmail.Authenticate(string username, string password)
        {
            try
            {
                mainImapClient.Authenticate(username, password);
                loadImapClient.Authenticate(username, password);
                smtpClient.Authenticate(username, password);
            }
            catch(Exception ex)
            {
                return ex;
            }
            activeUser.password = password;
            activeUser.username = username;
            // Preparing main window
            initialise();
            return null;
        }

        /// <summary>
        /// Used to maintain connection to the servers
        /// </summary>
        void MaintainConnection(object? sender, EventArgs e)
        {
            Debug.WriteLine("Pinging server to maintain connection");
            MainImapMutex.WaitOne();
            if(!mainImapClient.IsConnected)
            {
                mainImapClient.Connect(activeUser.IMAP_hostname, activeUser.IMAP_port, true);
            }
            if (!mainImapClient.IsAuthenticated)
            {
                mainImapClient.Authenticate(activeUser.username, activeUser.password);
            }
            try { mainImapClient.NoOp(); } catch { /*oh no*/ }
            
            MainImapMutex.ReleaseMutex();


            loadImapMutex.WaitOne();
            if (!loadImapClient.IsConnected)
            {
                loadImapClient.Connect(activeUser.IMAP_hostname, activeUser.IMAP_port, true);
            }
            if (!loadImapClient.IsAuthenticated)
            {
                loadImapClient.Authenticate(activeUser.username, activeUser.password);
            }
            try { loadImapClient.NoOp(); } catch { /*oh no*/ }

            loadImapMutex.ReleaseMutex();


            SmtpMutex.WaitOne();
            if (!smtpClient.IsConnected)
            {
                smtpClient.Connect(activeUser.SMTP_hostname, activeUser.SMTP_port, true);
            }
            if (!smtpClient.IsAuthenticated)
            {
                smtpClient.Authenticate(activeUser.username, activeUser.password);
            }
            try { smtpClient.NoOp(); } catch { /*oh no*/ }

            SmtpMutex.ReleaseMutex();
        }
        #endregion

         //__________________________________
        // Functions that access the database
        #region database functions

        IDatabase IEmail.GetDatabase()
        {
            return data;
        }
        void IEmail.SetDatabase(IDatabase database)
        {
            data = database;
        }

        List<string> IEmail.GetUsernames()
        {
            var users = data.GetUsers();
            List<string> usernames = new();
            foreach(var user in users)
            {
                usernames.Add(user.username.ToString());
            }
            return usernames;
        }

        IDatabase.User? IEmail.GetUser(string username)
        {
            return data.GetUser(username);
        }

        IDatabase.User IEmail.GetDefaultUser()
        {
            return data.GetDefaultUser();
        }

        int IEmail.SetDefaultUser(string username)
        {
            return data.SetDefaultUser(username);
        }

        int IEmail.DeleteUser(string username)
        {
            return data.DeleteUser(username);
        }

        int IEmail.SaveUser(IDatabase.User saveUser)
        {
            return data.SaveUser(saveUser);
        }
        #endregion

         //____________________________________________
        // Functions that supply client info to the GUI
        #region client data retrieval functions
        IDatabase.User IEmail.GetActiveUser()
        {
            return activeUser;
        }

        void IEmail.UpdateFolder(int folderIndex)
        {
            updateFolder(folderIndex);
        }

        void IEmail.UpdateFolderAsync(int folderIndex)
        {
            ThreadPool.QueueUserWorkItem(state => updateFolder(folderIndex));
        }

        void IEmail.UpdateInbox()
        {
            updateFolder(0);
        }
        void IEmail.UpdateInboxAsync()
        {
            ThreadPool.QueueUserWorkItem(state => updateFolder(0));
        }
        public event EventHandler FolderUpdateFinished;

        int IEmail.LoadSetActiveFolder(int folderIndex)
        {
            var folder = emailFolders[folderIndex];

            // If folder is locked, return
            if (folder.isLocked)
                return -1;

            // Else, set active with load if not loaded yet
            if(!folder.isLoaded)
            {
                updateFolder(folderIndex);
            }

            activeFolder = folder;

            return 0;
        }

        List<(string UID, string from, string date, string subject, bool read)> IEmail.GetFolderEnvelopes(int folderIndex, int amount)
        {
            List<(string, string, string, string, bool)> envelopes = new();

            var folder = emailFolders[folderIndex];

            if (folder != null)
            {
                // The envelopes up to this index are already loaded
                int StartIndex = folder.EnvelopesLoaded;
                int EndIndex = Math.Min(StartIndex + amount, folder.Envelopes.Count - folder.NewMessagesAdded);

                int i;
                for (i = StartIndex; i < EndIndex; i++)
                {
                    var UID = folder.UIDs[i];
                    envelopes.Add((UID.ToString(),
                                   folder.Envelopes[UID].from,
                                   folder.Envelopes[UID].date,
                                   folder.Envelopes[UID].subject,
                                   folder.Seen[UID]));
                }

                // Updating loaded count
                folder.EnvelopesLoaded += i - StartIndex;
            }

            return envelopes;
        }

        List<(string UID, string from, string date, string subject, bool read)> IEmail.GetNewMessageEnvelopes()
        {
            List<(string, string, string, string, bool)> envelopes = new();

            MainImapMutex.WaitOne();

            var inbox = emailFolders[0];

            inbox.MainImapFolder.Open(FolderAccess.ReadWrite);

            foreach (var items in inbox.MainImapFolder.Fetch(0, -1,
                                                       MessageSummaryItems.Envelope |
                                                       MessageSummaryItems.UniqueId |
                                                       MessageSummaryItems.Flags))
            {
                // If message is not already loaded, it is new
                if (!inbox.UIDs.Contains(items.UniqueId))
                {
                    Debug.WriteLine("Message not currently loaded. Adding to list");
                    inbox.UIDs.Add(items.UniqueId);
                    inbox.Seen.Add(items.UniqueId, items.Flags!.Value.HasFlag(MessageFlags.Seen));

                    IEmail.Message envelope = new();
                    envelope.from = items.Envelope.From.ToString();
                    if (items.Envelope.Date != null)
                        envelope.date = items.Envelope.Date.Value.LocalDateTime.ToString();
                    if (items.Envelope.Subject != null)
                        envelope.subject = items.Envelope.Subject.ToString();
                    inbox.Envelopes.Add(items.UniqueId, envelope);


                    envelopes.Add((items.UniqueId.ToString(), envelope.from, envelope.date, envelope.subject,
                                   items.Flags!.Value.HasFlag(MessageFlags.Seen)));

                    inbox.NewMessagesAdded++;
                }
            }

            MainImapMutex.ReleaseMutex();

            return envelopes;

        }

        List<(string UID, string from, string date, string subject, bool read)> IEmail.GetAllFolderEnvelopes(int folderIndex)
        {
            return ((IEmail)this).GetFolderEnvelopes(folderIndex, int.MaxValue);
        }


        IEmail.Message? IEmail.OpenMessage(string sUID)
        {
            // Adding read flag
            var UID = UniqueId.Parse(sUID);

            if (!activeFolder!.Seen[UID])
            {
                ToggleReadQueue.Enqueue(new ToggleReadParameters() {folder = activeFolder!,
                                                                    ID = UID,
                                                                    seenFlag = false  });
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
            }

            // Getting message. If it was not already loaded, it is added to the local folder
            MimeMessage ImapMessage;
            if(activeFolder.Messages.ContainsKey(UID))
                ImapMessage = activeFolder.Messages[UID];
            else
            {
                MainImapMutex.WaitOne();

                if (!activeFolder!.MainImapFolder!.IsOpen) { }
                    activeFolder!.MainImapFolder.Open(FolderAccess.ReadWrite);

                Debug.WriteLine("Message not loaded. Loading...");
                ImapMessage = activeFolder!.MainImapFolder!.GetMessage(UID);
                activeFolder.Messages[UID] = ImapMessage;
                Debug.WriteLine("Message with subject {0} loaded", ImapMessage.Subject);

                MainImapMutex.ReleaseMutex();
            }
            IEmail.Message message = new();
            message.date    = ImapMessage.Date.ToString();
            message.from    = ImapMessage.From.ToString();
            message.to      = ImapMessage.To.ToString();
            if (ImapMessage.Cc != null)
                message.cc      = ImapMessage.Cc.ToString();
            if (ImapMessage.Subject != null)
                message.subject = ImapMessage.Subject;
            if (ImapMessage.TextBody != null)
                message.body    = ImapMessage.TextBody.ToString();
            if (ImapMessage.Attachments != null)
            {
                foreach(var attachment in ImapMessage.Attachments)
                {
                    if(attachment is MessagePart)
                    {
                        message.attachments.Add(attachment.ContentDisposition.FileName);
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        message.attachments.Add(part.FileName);
                    }
                }
            }
            return message;
        }

        void IEmail.WriteAttachmentToFile(ref Stream Stream, string sUID, string fileName)
        {
            var UID = UniqueId.Parse(sUID);
            var ImapMessage = activeFolder!.Messages[UID];
            if (ImapMessage.Attachments != null)
            {
                foreach (var attachment in ImapMessage.Attachments)
                {
                    if (attachment is MessagePart)
                    {
                        if(attachment.ContentDisposition.FileName == fileName)
                        {
                            var rfc = (MessagePart)attachment;
                            rfc.Message.WriteTo(Stream);
                        }
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        if(part.FileName == fileName)
                        {
                            part.Content.DecodeTo(Stream);
                        }
                    }
                }
            }
        }

        List<string> IEmail.GetFoldernames()
        {
            List<string> folderNames = new();
            foreach (var folder in emailFolders)
            {
                folderNames.Add(folder.MainImapFolder!.Name);
            }
            if(trashFolder == null)
            {
                NoTrashFolderDetected(null, EventArgs.Empty);
            }
            return folderNames;
        }
        public event EventHandler NoTrashFolderDetected;
        #endregion

         //__________________________________________
        // Functions that make changes in IMAP server
        #region IMAP server changing functions
        /// <summary>
        /// This class and the corresponding queue is used to execute 
        /// asynchronous toggle read instructions in FIFO order.
        /// </summary>
        public class ToggleReadParameters
        {
            public EmailFolder? folder;
            public UniqueId     ID;
            public bool         seenFlag;
        }
        /// <summary>
        /// ToggleReadParameters must be enqueued before the AsyncToggleRead function is called.
        /// </summary>
        Queue<ToggleReadParameters> ToggleReadQueue = new();

        void IEmail.ToggleRead(string sUID)
        {
            Debug.WriteLine("Running MailKitEmail.ToggleRead()");
            var UID = UniqueId.Parse(sUID);
            var seen = activeFolder!.Seen[UID];
            if (!seen)
            {
                activeFolder.Seen[UID] = true; // Updating local data
                Debug.WriteLine("Locally adding flag");
            }  
            else
            {
                activeFolder.Seen[UID] = false;
                Debug.WriteLine("Locally removing flag");
            }
            ToggleReadQueue.Enqueue(new ToggleReadParameters()
            {
                folder = activeFolder!,
                ID = UID,
                seenFlag = seen
            });
            ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));

            Debug.WriteLine("MailKitEmail.ToggleRead() complete");
        }
        private void AsyncToggleRead(object? sender, EventArgs e)
        {
            MainImapMutex.WaitOne();

            if (ToggleReadQueue.Count > 0)
            {

                // Gettings parameters
                var parameters = ToggleReadQueue.Dequeue();

                if (!parameters.folder!.MainImapFolder!.IsOpen)
                    parameters.folder.MainImapFolder.Open(FolderAccess.ReadWrite);

                if (!parameters.seenFlag)
                {
                    parameters.folder!.MainImapFolder.AddFlags(parameters.ID, MessageFlags.Seen, true);
                    Debug.WriteLine("Server request to add flag");
                }
                else
                {
                    parameters.folder!.MainImapFolder.RemoveFlags(parameters.ID, MessageFlags.Seen, true);
                    Debug.WriteLine("Server request to remove flag");
                }

                // Recursive call to handle next queued task if necessary
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
            }

            MainImapMutex.ReleaseMutex();
        }

        void IEmail.Delete(string sUID)
        {
            Debug.WriteLine("Running MailKitEmail.Delete()");
            var UID = UniqueId.Parse(sUID);

            // The local message entry will not be removed
            ThreadPool.QueueUserWorkItem(state => AsyncDelete(activeFolder!, UID));

            Debug.WriteLine("MailKitEmail.Delete() complete");
        }
        private void AsyncDelete(EmailFolder folder, UniqueId ID)
        {
            MainImapMutex.WaitOne();

            if(!folder.MainImapFolder!.IsOpen)
            {
                folder.MainImapFolder.Open(FolderAccess.ReadWrite);
            }

            // If trash folder is detected, move the message
            if (trashFolder != null & folder != trashFolder)
                MoveMessage(ID, trashFolder.FolderIndex, false);
            else
            {
                folder.MainImapFolder.AddFlags(ID, MessageFlags.Deleted, true);
                folder.MainImapFolder.Expunge();
            }

            Debug.WriteLine("Server request to add deleted flag");

            MainImapMutex.ReleaseMutex();
        }

        void IEmail.MoveMessage(string sUID, int folderIndex)
        {
            MoveMessage(UniqueId.Parse(sUID), folderIndex, true);
        }
        private void MoveMessage(UniqueId UID, int folderIndex, bool async)
        {
            Debug.WriteLine("Running MailKitEmail.MoveMessage() with destination "
               + emailFolders[folderIndex].MainImapFolder!.Name);

            // Saving message data before move
            var Envelope = activeFolder!.Envelopes[UID];
            var Seen = activeFolder.Seen[UID];
            MimeMessage? message = null;
            if (activeFolder.Messages.ContainsKey(UID))
                message = activeFolder.Messages[UID];

            if (async)
                ThreadPool.QueueUserWorkItem(state => AsyncMoveMessage(UID, folderIndex, activeFolder!,
                                                                       Envelope, Seen, message));
            else
                AsyncMoveMessage(UID, folderIndex, activeFolder!, Envelope, Seen, message);

            Debug.WriteLine("MailKitEmail.MoveMessage() complete");
        }
        private void AsyncMoveMessage(UniqueId UID, int destinationIndex, EmailFolder origin, 
                                      IEmail.Message Envelope, bool Seen, MimeMessage? Message)
        {
            MainImapMutex.WaitOne();
            var destinationFolder = emailFolders[destinationIndex];

            if (!origin.MainImapFolder!.IsOpen)
            {
                origin.MainImapFolder.Open(FolderAccess.ReadWrite);
            }

            Debug.WriteLine("Server request to move message.");

            UniqueId destinationUID = UID;
            bool succes = true;
            Exception? Exception = null;

            try
            {
                destinationUID = origin!.MainImapFolder!.MoveTo(UID, destinationFolder.MainImapFolder)!.Value;
            }
            catch(Exception ex)
            {
                // If the operation failed, the message will be returned to the original folder.
                destinationUID = UID;
                destinationFolder = origin;
                destinationIndex = origin.FolderIndex;
                succes = false;
                Exception = ex;
            }

            // Adding message information to new folder
            destinationFolder.UIDs.Add(destinationUID);
            if (!destinationFolder.Envelopes.ContainsKey(destinationUID))
                destinationFolder.Envelopes[destinationUID] = Envelope;
            if (!destinationFolder.Seen.ContainsKey(destinationUID))
                destinationFolder.Seen[destinationUID] = Seen;
            if (!destinationFolder.Messages.ContainsKey(destinationUID) & Message != null)
                destinationFolder.Messages[destinationUID] = Message!;

            // Removing the message from the origin folder locally:
            if(succes)
            {
                if(origin.Messages.ContainsKey(UID))
                    origin.Messages.Remove(UID);
                origin.Seen.Remove(UID);
                origin.Envelopes.Remove(UID);
            }

            MainImapMutex.ReleaseMutex();

            Debug.WriteLine("Sending MoveMessageFinished event for message with subject " + Envelope.subject);
            this.MoveMessageFinished(destinationUID.ToString()!,
                                     destinationIndex,
                                     Envelope,
                                     Seen,
                                     succes, Exception);
        }
        public event IEmail.MoveMessageFinishedHandler MoveMessageFinished;


        Exception? IEmail.AddFolder(string folderName)
        {
            MainImapMutex.WaitOne();

            var topFolder = mainImapClient.GetFolder(mainImapClient.PersonalNamespaces[0]);
            IMailFolder newMainFolder = topFolder.Create(folderName, false);
            IMailFolder? newLoadFolder = null;

            //wack solution, but time is short
            var allFolders = mainImapClient.GetFolders(mainImapClient.PersonalNamespaces[0]);
            foreach(var folder in allFolders)
            {
                if(folder.Name == folderName)
                {
                    newLoadFolder = folder;
                    emailFolders.Add(new EmailFolder(newMainFolder, newLoadFolder, folderName));
                }
            }

            MaintainConnection(null, EventArgs.Empty);

            MainImapMutex.ReleaseMutex();

            if (newLoadFolder == null) return new Exception("Folder could not be properly created. Try again.");

            return null; // Success
        }

        Exception? IEmail.DeleteFolder(string oldName, int folderIndex)
        {
            MainImapMutex.WaitOne();

            IMailFolder? deleteFolder = null;
            foreach (var folder in mainImapClient.GetFolders(mainImapClient.PersonalNamespaces[0]))
            {
                if (folder.Name == oldName)
                {
                    deleteFolder = folder;
                    break;
                }
            }
            if (deleteFolder == null) return new Exception("Delete folder failed.");

            try
            {
                deleteFolder.Delete();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return ex;
            }

            emailFolders.RemoveAt(folderIndex);

            MaintainConnection(null, EventArgs.Empty);

            MainImapMutex.ReleaseMutex();

            return null; // Success
        }

        Exception? IEmail.RenameFolder(string oldName, string newName, int folderIndex)
        {
            MainImapMutex.WaitOne();

            IMailFolder? renamedFolder = null;
            foreach(var folder in mainImapClient.GetFolders(mainImapClient.PersonalNamespaces[0]))
            {
                if (folder.Name == oldName)
                {
                    renamedFolder = folder;
                    break;
                }
            }
            if(renamedFolder == null) return new Exception("Rename folder failed.");

            try
            {
                renamedFolder.Rename(renamedFolder.ParentFolder, newName);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                return ex;
            }

            emailFolders[folderIndex].FolderName = newName;

            MaintainConnection(null, EventArgs.Empty);

            MainImapMutex.ReleaseMutex();

            return null; // Success
        }

        #endregion

        //____________________________________________
        // Functions that interact with the SMTP server 
        #region SMTP functions

        void IEmail.SendMessage(IEmail.Message message, List<(Stream stream, string filename)> attachments)
        {
            var addresses     = SplitEmails(message.to);
            var cc_addresses  = SplitEmails(message.cc);
            var bcc_addresses = SplitEmails(message.bcc);
            List<MimeMessage> messages = new();

            // If the strings are invalid, an exception will be thrown.
            foreach(var address in addresses)
            {
                var MimeMsg = new MimeMessage();
                try
                {
                    MimeMsg.From.Add(new MailboxAddress("", activeUser.username));
                    MimeMsg.To.Add(new MailboxAddress("", address));
                    if (message.cc.Length > 0)
                        foreach(var cc_address in cc_addresses)
                            MimeMsg.Cc.Add(new MailboxAddress("", cc_address));
                    if (message.bcc.Length > 0)
                        foreach (var bcc_address in bcc_addresses)
                            MimeMsg.Bcc.Add(new MailboxAddress("", bcc_address));
                    MimeMsg.Subject = message.subject;

                    // Creating body
                    var body = new BodyBuilder { 
                        HtmlBody = message.body,
                        TextBody = message.body
                        };

                    // Adding attachments to body
                    if(attachments != null)
                    {
                        foreach (var parAtt in attachments)
                        {
                            var attachment = new MimePart()
                            {
                                Content = new MimeContent(parAtt.stream, ContentEncoding.Default),
                                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                ContentTransferEncoding = ContentEncoding.Base64,
                                FileName = parAtt.filename
                            };
                            body.Attachments.Add(attachment);
                        }
                    }
                    
                    // Adding body to message
                    MimeMsg.Body = body.ToMessageBody();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Send message syntax error: " + ex.ToString());
                    if (addresses.Count > 1)
                        ex.Data.Add("Multi receiver email failed",
                                    "At least one email address of an email with multiple receivers returned an error. "
                                  + "The email was not send to any of the addresses in the receiver list. "
                                  + "Verify addresses and try again."); 
                    this.SentMessage(ex, message);
                    return;
                }
                messages.Add(MimeMsg);
            }

            foreach(var MimeMsg in messages)
                ThreadPool.QueueUserWorkItem(state => AsyncSendMessage(MimeMsg, message));
        }
        private void AsyncSendMessage(MimeMessage MimeMsg, IEmail.Message message)
        {
            SmtpMutex.WaitOne();
            try
            {
                smtpClient.Send(MimeMsg);
            }
            catch (Exception? ex)
            {
                Debug.WriteLine("AsyncSendMessage server exception: " + ex.ToString());
                this.SentMessage(ex, message);
                SmtpMutex.ReleaseMutex();
                return;
            }
            SmtpMutex.ReleaseMutex();
            this.SentMessage(null, message);
        }
        public event IEmail.SentMessageHandler SentMessage;

        #endregion

    }
}
