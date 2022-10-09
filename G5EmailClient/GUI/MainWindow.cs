
using G5EmailClient.Email;

namespace G5EmailClient.GUI
{
    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;

        public MainWindow(IEmail ParamEmailClient)
        {
            InitializeComponent();

            AddDraggingControl(this);

            // Saving email client to local variable
            EmailClient = ParamEmailClient;

            // Opening connection form
            this.Visible = true; // Set to true for testing, should be false
            Application.Run(new ConnectionForm(EmailClient));
            this.Visible = true;


        }
    }
}