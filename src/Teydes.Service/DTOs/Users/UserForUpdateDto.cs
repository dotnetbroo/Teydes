using System.ComponentModel.DataAnnotations;
using Teydes.Domain.Commons;
using Teydes.Domain.Enums;

namespace Teydes.Service.DTOs.Users;

public class UserForUpdateDto 
{

    [Required(ErrorMessage = "FirstName is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "LastName is required")]
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Place of study required")]
    public bool IsStudyForeign { get; set; }
}
