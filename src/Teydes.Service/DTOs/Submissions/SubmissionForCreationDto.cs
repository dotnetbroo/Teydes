namespace Teydes.Service.DTOs.Submissions;

public class SubmissionForCreationDto
{
    public long UserId { get; set; }
    public long QuestionId { get; set; }
    public long QuizId { get; set; }
    public long QuestionOptionId { get; set; }
}
