using Teydes.Domain.Enums;
using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Entities.Courses;
using System.Text.Json.Serialization;

namespace Teydes.Domain.Entities.Users;

public class User : Auditable
{
    public long? TelegramId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public UserRole Role { get; set; }
    public bool IsStudyForeign { get; set; }

    [JsonIgnore]
    public ICollection<UserGroup> UserGroups { get; set; }
    public ICollection<QuizResult> QuizResults { get; set; }
    public ICollection<Submission> Submissions { get; set; }
}
