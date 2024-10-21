using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Teydes.Service.Commons.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PhoneNumberAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string phoneNumber = (string)value;
        if (String.IsNullOrWhiteSpace(phoneNumber))
        {
            return new ValidationResult("Iltimos, telefon raqamini to`g`ri kiriting!");
        }
        else
        {
            Regex regex = new Regex("^(?:\\+998([- ])?(90|91|93|94|95|98|99|33|97|71|99|88)([- ])?(\\d{3})([- ])?(\\d{2})([- ])?(\\d{2}))");

            return (regex.Match(phoneNumber).Success) ? ValidationResult.Success
                : new ValidationResult("Iltimos, telefon raqamini to`g`ri kiriting!");
        }
    }
}
