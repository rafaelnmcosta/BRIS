using System.Net.Mail;

namespace bris_API.Services
{
    public class EmailService : IEmailService
    {
        public async Task EnviarEmailRecuperacaoSenha(string email, string novaSenha)
        {
            var remetente = new MailAddress("bris.suporte@gmail.com", "Suporte BRIS");
            var destino = new MailAddress(email);
            const string assunto = "Recuperação de Senha - BRIS";
            string corpo = $"Sua nova senha é: {novaSenha}.";

            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(remetente.Address, "ojhnvldjueenmjnk")
            })
            {
                using (var message = new MailMessage(remetente, destino)
                {
                    Subject = assunto,
                    Body = corpo
                })
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
