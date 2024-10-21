
using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Courses;

namespace Teydes.Domain.Entities.Quizes;

public class Quiz : Auditable
{
    public string Title { get; set; }
    public int TimeToSolveInMinute { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long GroupId { get; set; }
    public Group Group { get; set; }

    public ICollection<Question> Questions { get; set; }
    public ICollection<QuizResult> QuizResults { get; set; }
    public ICollection<Submission> Submissions { get; set; }

}
