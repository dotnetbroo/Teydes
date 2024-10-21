using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Users;

namespace Teydes.Domain.Entities.Quizes;

public class QuizResult : Auditable
{
    public int CorrectAnswers { get; set; }
    public double Score { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public long QuizId { get; set; }
    public Quiz Quiz { get; set; }

}
