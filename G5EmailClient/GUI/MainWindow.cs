
using G5EmailClient.Email;

namespace G5EmailClient.GUI
{
    public partial class MainWindow : Form
    {
        IEmail EmailClient;

        public MainWindow(IEmail ParamEmailClient)
        {
            InitializeComponent();

            // Saving email client to local variable
            EmailClient = ParamEmailClient;

            // Opening connection form
            this.Visible = false;
            Application.Run(new ConnectionForm(EmailClient));
            this.Visible = true;


        }
    }
}