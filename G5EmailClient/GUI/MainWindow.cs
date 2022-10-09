
using System.Diagnostics;

using G5EmailClient.Email;

namespace G5EmailClient.GUI
{
    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;

        List<EnvelopePanel> envelopePanels = new();

        public MainWindow(IEmail ParamEmailClient)
        {
            InitializeComponent();

            // Enabling window dragging for controls
            AddDraggingControl(this);
            AddDraggingControl(top_toolstrip);

            // Saving email client to local variable
            EmailClient = ParamEmailClient;

            // Opening connection form
            this.Visible = false;
            Application.Run(new ConnectionForm(EmailClient));
            this.Visible = true;

            // Setting up GUI
            main_tab.TabPages.Remove(compose_message_tab);
            main_tab.TabPages.Remove(open_message_tab);
            main_tab.Appearance = TabAppearance.FlatButtons;
            main_tab.ItemSize = new Size(0, 1);
            main_tab.SizeMode = TabSizeMode.Fixed;

            // Initializing email data.
            updateInboxView();
            active_email_label.Text = EmailClient.GetActiveUser().username;
        }

        /// <summary>
        /// Gets message envelopes for the active inbox and adds them to the inbox_view.
        /// Ideally this will be done asynchronously.
        /// </summary>
        void updateInboxView()
        {
            // Clearing inbox
            foreach (var envelope in envelopePanels) envelope.Dispose();
            envelopePanels.Clear();

            // The internal index value of the envelope will match the index in the envelopePanels list
            int index = -1;
            var envelopes = EmailClient.GetFolderEnvelopes();
            foreach (var envelope in envelopes)
            {
                index++;
                var envelopePanel = new EnvelopePanel();
                    envelopePanel.index = index;
                    envelopePanel.fromText = envelope.from.ToString();
                    envelopePanel.subjectText = envelope.subject;
                    if (!envelope.read)
                        envelopePanel.colorBar = Color.Black;
                    envelopePanel.Anchor = AnchorStyles.Top;
                    envelopePanel.AutoSize = true;
                    envelopePanel.MinimumSize = new Size(envelopes_flowpanel.Width - 6 - SystemInformation.VerticalScrollBarWidth, 68);
                    envelopePanel.DoubleClick += new EventHandler(EnvelopePanel_DoubleClick);
                // Adding the control to the window
                envelopes_flowpanel.Controls.Add(envelopePanel);
                // Adding to list for later use
                envelopePanels.Add(envelopePanel);

                // this.env_from_label.Click += new System.EventHandler(this.EnvelopePanel_Click);
            }
        }

        private void EnvelopePanel_DoubleClick(object sender, EventArgs e)
        {
            var envelope = (EnvelopePanel)sender;
            var message = EmailClient.GetMessage(envelope.index);
            // Test
            Debug.WriteLine("Opening message " + envelope.index.ToString() + ": " 
                          + message.from + message.subject + message.body);
        }
    }
}