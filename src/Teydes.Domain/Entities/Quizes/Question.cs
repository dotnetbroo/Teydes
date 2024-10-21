using Teydes.Domain.Enums;
using Teydes.Domain.Commons;
using Teydes.Domain.Entities.Assets;

namespace Teydes.Domain.Entities.Quizes;

public class Question : Auditable
{
    public long QuizId { get; set; }
    public Quiz Quiz { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public long? AssetId { get; set; }
    public Asset Asset { get; set; }

    public ICollection<QuestionAnswer> QuestionAnswers { get; set; }
    public ICollection<Submission> Submissions { get; set; }

}
