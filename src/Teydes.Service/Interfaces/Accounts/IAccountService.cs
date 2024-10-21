using Teydes.Service.DTOs.Logins;

namespace Teydes.Service.Interfaces.Accounts;

public interface IAccountService
{
    public Task<string> LoginAsync(LoginDto loginDto);
}
