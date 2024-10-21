using Teydes.Domain.Entities.Assets;
using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Enums;
using Teydes.Service.DTOs.Answers;

namespace Teydes.Service.DTOs.Questions;

public class QuestionForQuizResultDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public long? AssetId { get; set; }

    public ICollection<QuestionAnswerForResultDto> QuestionAnswers { get; set; }
}
