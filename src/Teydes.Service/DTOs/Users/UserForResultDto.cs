using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Enums;
using Teydes.Service.DTOs.Groups;
using Teydes.Service.DTOs.Quizzes;

namespace Teydes.Service.DTOs.Users;

public class UserForResultDto
{
    public long Id { get; set; }
    public long? TelegramId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserRole Role { get; set; }
    public bool IsStudyForeign { get; set; }


    public ICollection<QuizForResultDto> LastQuizzes { get; set; }
    public ICollection<GroupForResultDto> Groups { get; set; }

}
