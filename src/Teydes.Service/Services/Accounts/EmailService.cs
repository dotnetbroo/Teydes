using MailKit.Net.Smtp;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Teydes.Service.Interfaces.Accounts;
using Teydes.Service.Services.Accounts.Models;

namespace Teydes.Service.Services.Accounts;

public class EmailService : IEmailService
{
    private readonly IConfiguration configuration;
    private readonly IMemoryCache memoryCache;

    public EmailService(IConfiguration configuration, IMemoryCache memoryCache)
    {
        this.configuration = configuration.GetSection("Email");
        this.memoryCache = memoryCache;
    }

    public bool VerifyCode(string email, string code)
    {
        var cashedValue = memoryCache.Get<string>(email);

        if (cashedValue?.ToString() == code)
        {
            return true;
        }

        return false;
    }

    public async Task SendMessageAsync(Message message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(configuration["EmailAddress"]));
        email.To.Add(MailboxAddress.Parse(message.To));

        email.Subject = message.Subject;
        email.Body = new TextPart("html")
        {
            Text = message.Body
        };

        var smtp = new SmtpClient();

        await smtp.ConnectAsync(configuration["Host"], 587, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(configuration["EmailAddress"], configuration["Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    public async Task<bool> SendCodeByEmailAsync(string email)
    {
        var randomNumber = new Random().Next(100000, 999999);

        var message = new Message()
        {
            Subject = "Do not give this code to Others",
            To = email,
            Body = $"{randomNumber}"
        };

        memoryCache.Set(email, randomNumber.ToString(), TimeSpan.FromMinutes(2));
        await this.SendMessageAsync(message);

        return true;
    }
}
