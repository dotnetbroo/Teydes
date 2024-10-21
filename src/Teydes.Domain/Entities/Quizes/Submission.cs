using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Users;

namespace Teydes.Domain.Entities.Quizes;

public class Submission : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }  
    public bool IsCorrect { get; set; }
    public long QuestionId { get; set; }
    public Question Question { get; set;}
    public long QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public long QuestionOptionId { get; set; }
    public QuestionAnswer QuestionOption { get; set;}
}
