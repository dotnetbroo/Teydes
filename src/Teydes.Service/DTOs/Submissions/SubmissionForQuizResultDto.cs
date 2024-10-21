using Teydes.Service.DTOs.Answers;
using Teydes.Service.DTOs.Questions;

namespace Teydes.Service.DTOs.Submissions;

public class SubmissionForQuizResultDto
{
    public long Id { get; set; }
    public QuestionForResultDto Question { get; set; }
    public QuestionAnswerForResultDto QuestionOption { get; set; }
}
