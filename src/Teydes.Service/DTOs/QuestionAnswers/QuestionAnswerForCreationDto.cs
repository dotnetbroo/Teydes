using System.ComponentModel.DataAnnotations;

namespace Teydes.Service.DTOs.Answers;

public class QuestionAnswerForCreationDto
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }

    [Required(ErrorMessage = "QuestionId is required")]
    public long QuestionId { get; set; }
}
