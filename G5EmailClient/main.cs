
using G5EmailClient.Email;
using G5EmailClient.GUI;

namespace G5EmailClient
{
    internal static class main
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IEmail EmailClient = new MailKitEmail();

            Application.Run(new MainWindow(EmailClient));
        }
    }
}