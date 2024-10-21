using System.ComponentModel.DataAnnotations;
using Teydes.Service.Commons.Validations;

namespace Teydes.Service.Commons.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class StrongPasswordAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return new ValidationResult("Parol majburiy");
        else
        {
            string password = value.ToString()!;
            if (password.Length < 8)
                return new ValidationResult("Parol 8 ta belgidan kam bo'lmasligi kerak");
            else if (password.Length > 30)
                return new ValidationResult("Parol 30 ta belgidan ko'p bo'lmasligi kerak");
            var result = PasswordValidator.IsStrong(password);

            if (result.IsValid is false) return new ValidationResult(result.Message);
            else return ValidationResult.Success;
        }
    }
}
