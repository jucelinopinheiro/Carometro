using CarometroV7.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

namespace CarometroV7.Helper
{
    public class Email
    {
        private readonly IConfiguration _configuration;
        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Enviar(string assunto, string mensagem, string anexo, List<Usuario> usuarios)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string mailaddress = _configuration.GetValue<string>("SMTP:MailAddress");
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string userName = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int port = _configuration.GetValue<int>("SMTP:Port");

                //novo objeto MailMessage
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(mailaddress, nome)
                };

                //preparo para envio
                foreach (var usuario in usuarios)
                {
                    if (!string.IsNullOrEmpty(usuario.Email))
                        mail.To.Add(usuario.Email);
                }
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                if (!string.IsNullOrEmpty(anexo))
                {
                    Attachment anexos = new Attachment(anexo, MediaTypeNames.Application.Octet);
                    mail.Attachments.Add(anexos);
                }

                //trabalho para envio
                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(userName, senha);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
