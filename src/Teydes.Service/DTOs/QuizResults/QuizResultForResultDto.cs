using Teydes.Service.DTOs.Answers;
using Teydes.Service.DTOs.Questions;
using Teydes.Service.DTOs.Submissions;

namespace Teydes.Service.DTOs.QuizResults;

public class QuizResultForResultDto
{
    public int CorrectAnswers { get; set; }
    public int Score { get; set; }

    public ICollection<SubmissionForQuizResultDto> Submissions { get; set; }
    public ICollection<QuestionForQuizResultDto> Questions { get; set; }
}
