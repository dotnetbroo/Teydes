using System.ComponentModel.DataAnnotations;
using Teydes.Service.Commons.Attributes;

namespace Teydes.Service.DTOs.Logins;

public class LoginDto
{
    [Required(ErrorMessage = "Telefon raqamni kiriting"), PhoneNumber]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Parolni kiriting"), StrongPassword]
    public string Password { get; set; }
}
