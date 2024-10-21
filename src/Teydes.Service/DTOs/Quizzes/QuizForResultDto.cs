using Teydes.Service.DTOs.Groups;

namespace Teydes.Service.DTOs.Quizzes;

public class QuizForResultDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public int TimeToSolveInMinute { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public GroupForResultDto Group { get; set; }
}
