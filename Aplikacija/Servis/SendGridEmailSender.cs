using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Grupa5Tim3.servis;
public class SendGridOptions
{
    public string ApiKey { get; set; }
}

public class SendGridEmailSender 
{
    private readonly SendGridOptions _options;

    public SendGridEmailSender(IOptions<SendGridOptions> options)
    {
        _options = options.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress("lamijabojic@gmail.com", "Artevo");
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: " ", htmlContent: htmlMessage);
        var response = await client.SendEmailAsync(msg);
        Console.WriteLine($"Status Code: {response.StatusCode}");

        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            Console.WriteLine($"Failed to send email. Response body: {responseBody}");
        }
    }

    public async Task<string> SendVerificationCodeEmailAsync(string email)
    {
        var verificationCode = new Random().Next(100000, 999999).ToString(); 

        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress("lamijabojic@gmail.com", "Artevo");
        var to = new EmailAddress(email);

        string subject = "Verifikacijski kod";
        string htmlMessage = $"<p>Vaš verifikacijski kod je: <strong>{verificationCode}</strong></p>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, verificationCode, htmlMessage);
        var response = await client.SendEmailAsync(msg);

        Console.WriteLine($"Status Code: {response.StatusCode}");
        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            Console.WriteLine($"Failed to send email. Response body: {responseBody}");
        }

        return verificationCode; 
    }

    public async Task SendQrCodeEmailAsync(string email, string subject, string htmlMessage, byte[] qrCodeBytes)
    {
        var client = new SendGridClient(_options.ApiKey);
        var from = new EmailAddress("lamijabojic@gmail.com", "Artevo");
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "", htmlContent: htmlMessage);

        // Dodaj QR kod kao attachment
        msg.AddAttachment("racun_qr.png", Convert.ToBase64String(qrCodeBytes), "image/png", "attachment");

        var response = await client.SendEmailAsync(msg);

        Console.WriteLine($"Status Code: {response.StatusCode}");
        if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            Console.WriteLine($"Failed to send email. Response body: {responseBody}");
        }
    }


}
