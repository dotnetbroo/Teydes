
using System.Text.Json.Serialization;
using Teydes.Service.DTOs.Questions;

namespace Teydes.Service.DTOs.Answers;

public class QuestionAnswerForResultDto
{
    public long Id { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public QuestionForResultDto Question { get; set; }
}
