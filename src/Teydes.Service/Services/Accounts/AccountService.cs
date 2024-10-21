using Teydes.Data.IRepositories;
using Teydes.Service.DTOs.Logins;
using Teydes.Domain.Entities.Users;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.Interfaces.Commons;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Interfaces.Accounts;

namespace Teydes.Service.Services.Accounts;

public class AccountService : IAccountService
{
    private readonly IAuthService authService;
    private readonly IRepository<User> userRepository;

    public AccountService(IRepository<User> userRepository, IAuthService authService)
    {
        this.authService = authService;
        this.userRepository = userRepository;
    }
    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await this.userRepository.SelectAsync(x => x.PhoneNumber == loginDto.PhoneNumber);
        if (user is null)
            throw new CustomException(404, "Telefor raqam yoki parol xato kiritildi!");

        var hasherResult = PasswordHelper.Verify(loginDto.Password, user.Salt, user.Password);
        if (hasherResult == false)
            throw new CustomException(404, "Telefor raqam yoki parol xato kiritildi!");

        return authService.GenerateToken(user);
    }
}
