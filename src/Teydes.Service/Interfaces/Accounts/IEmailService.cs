using Teydes.Service.Services.Accounts.Models;

namespace Teydes.Service.Interfaces.Accounts;

public interface IEmailService
{
    public Task SendMessageAsync(Message message);

    public Task<bool> SendCodeByEmailAsync(string email);

    public bool VerifyCode(string email, string code);
}
