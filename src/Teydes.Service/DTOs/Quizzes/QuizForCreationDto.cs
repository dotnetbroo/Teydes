using System.ComponentModel.DataAnnotations;

namespace Teydes.Service.DTOs.Quizzes;

public class QuizForCreationDto
{
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "TimeToSolveInMinue is required" )]
    public int TimeToSolveInMinute { get; set; }

    [Required(ErrorMessage = "StartDate is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "EndDate is required")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "GroupId is required")]
    public long GroupId { get; set; }
}
