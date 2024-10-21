using Teydes.Domain.Enums;

namespace Teydes.Service.DTOs.Users;

public class UserForGroupResultDto
{
    public long Id { get; set; }
    public long? TelegramId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }

}
