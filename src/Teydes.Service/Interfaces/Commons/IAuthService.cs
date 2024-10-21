using Teydes.Domain.Entities.Users;
using Teydes.Service.DTOs.Logins;

namespace Teydes.Service.Interfaces.Commons;

public interface IAuthService
{
    public string GenerateToken(User users);
}
