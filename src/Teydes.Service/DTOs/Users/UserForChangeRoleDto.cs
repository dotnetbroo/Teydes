using Teydes.Domain.Enums;

namespace Teydes.Service.DTOs.Users;

public class UserForChangeRoleDto
{
    public long Id { get; set; }
    public UserRole Role { get; set; }
}