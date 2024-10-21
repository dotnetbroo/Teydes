using Teydes.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Teydes.Service.DTOs.Questions;

public class QuestionForCreationDto
{
    [Required(ErrorMessage = "QuizId is required")]
    public long QuizId { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    public string Description { get; set; }

    [Required(ErrorMessage = "QuestionType is required")]
    public QuestionType Type { get; set; }
    public long? AssetId { get; set; }

}
