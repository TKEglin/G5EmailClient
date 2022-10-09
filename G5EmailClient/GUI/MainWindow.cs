
using G5EmailClient.Email;

namespace G5EmailClient.GUI
{
    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;

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

            // Initializing email data.
            updateInboxView();
        }

        /// <summary>
        /// Gets message envelopes for the active inbox and adds them to the inbox_view
        /// </summary>
        void updateInboxView()
        {
            var envelopes = EmailClient.GetFolderEnvelopes();
            foreach(var envelope in envelopes)
            {
                inbox_listview.Items.Add(new ListViewItem(new[] { envelope.from, envelope.subject }));
            }
        }
    }
}