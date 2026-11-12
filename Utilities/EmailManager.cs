using System.Net.Mail;
using System.Net;

namespace sof1_IIPA26.Utilities
{
    public class EmailManager
    {
        private readonly SmtpClient cliente;
        private MailMessage email;
        private readonly string Host = "smtp.gmail.com";
        private readonly int Port = 587;
        private readonly string User = "owlblack10@gmail.com";
        private readonly string Password = "qarihjhnrhakohry";//Contraseña de Aplicación
        private readonly bool EnabledSSL = true;

        public EmailManager()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }
        public void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false)
        {
            email = new MailMessage(User, destinatario, asunto, mensaje)
            {
                IsBodyHtml = esHtlm
            };
            cliente.Send(email);
            email.Dispose();
        }
    }
}
