using System.ComponentModel.DataAnnotations;

namespace Teydes.Service.DTOs.Users;

public class UserForChangePasswordDto
{
    [Required(ErrorMessage = "Old password must not be null or empty!")]
    public string OldPassword { get; set; }

    [Required(ErrorMessage = "New password must not be null or empty!")]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirming password must not be null or empty!")]
    public string ConfirmPassword { get; set; }
}
