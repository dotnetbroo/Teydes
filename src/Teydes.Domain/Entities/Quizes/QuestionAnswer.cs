using Teydes.Domain.Commons;

namespace Teydes.Domain.Entities.Quizes;

public class QuestionAnswer : Auditable
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public long QuestionId { get; set; }
    public Question Question { get; set; }

    ICollection<Submission> Submissions  { get; set; }

}
