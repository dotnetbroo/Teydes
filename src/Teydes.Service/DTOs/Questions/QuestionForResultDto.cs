using Teydes.Domain.Entities.Assets;
using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Enums;
using Teydes.Service.DTOs.Quizzes;

namespace Teydes.Service.DTOs.Questions;

public class QuestionForResultDto
{
    public long Id { get; set; }
    public QuizForResultDto Quiz { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public long? AssetId { get; set; }
}
