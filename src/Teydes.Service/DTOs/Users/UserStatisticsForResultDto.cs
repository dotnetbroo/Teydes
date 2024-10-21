using Teydes.Domain.Enums;
using Teydes.Service.DTOs.QuizResults;

namespace Teydes.Service.DTOs.Users;

public class UserStatisticsForResultDto
{
    public long Id { get; set; }
    public long? TelegramId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public bool IsStudyForeign { get; set; }

    public float TotalScore { get; set; }
    public int TestCount { get; set; }
    public ICollection<QuizResultForResultDto> QuizResults { get; set; }  
}
