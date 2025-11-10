using FoodSaver.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FoodSaver.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendEmail(string to, string subject, string htmlBody)
        {
            try
            {
                var client = new SendGridClient(_config["SendGrid:ApiKey"]);
                var from = new EmailAddress(_config["SendGrid:FromEmail"], _config["SendGrid:FromEmail"]);
                var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(to), subject, null, htmlBody);
                var response = await client.SendEmailAsync(msg);
                _logger.LogInformation($"The response status code is {response.StatusCode}");
                var responseBody = await response.Body.ReadAsStringAsync();
                _logger.LogInformation($"Response body: {responseBody}");
                return true;
            }catch (Exception ex)
            {
                _logger.LogError($"[EmailService] An error occured while sending the email to {to} : {ex}");
                return false;
            }
         

        }
    }
}
