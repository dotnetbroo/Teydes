namespace Teydes.Service.DTOs.Answers;

public class QuestionAnswerForUpdateDto
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public long QuestionId { get; set; }
}
