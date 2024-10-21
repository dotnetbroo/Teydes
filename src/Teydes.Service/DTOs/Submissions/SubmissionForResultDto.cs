using System.Text.Json.Serialization;
using Teydes.Service.DTOs.Answers;
using Teydes.Service.DTOs.Questions;
using Teydes.Service.DTOs.Quizzes;
using Teydes.Service.DTOs.Users;

namespace Teydes.Service.DTOs.Submissions;

public class SubmissionForResultDto
{
    public long Id { get; set; }
    public UserForGroupResultDto User { get; set; }
    public QuestionForResultDto Question { get; set; }
    public QuestionAnswerForResultDto QuestionOption {  get; set; }
}
