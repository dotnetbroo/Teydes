using Teydes.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Teydes.Service.DTOs.Questions;

public class QuestionForUpdateDto
{
    [Required(ErrorMessage = "QuizId is required")]
    public long QuizId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public QuestionType Type { get; set; }
    public long? AssetId { get; set; }

}
