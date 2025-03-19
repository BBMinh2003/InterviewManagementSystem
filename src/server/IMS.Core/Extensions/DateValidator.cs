using System.ComponentModel.DataAnnotations;

namespace IMS.Core.Extensions;

public static class DateValidator
{
    public static ValidationResult ValidatePastDate(DateTime? date, ValidationContext context)
    {
        if (!date.HasValue)
        {
            return new ValidationResult("Date of Birth is required.");
        }

        if (date.Value >= DateTime.Today)
        {
            return new ValidationResult("Date of Birth must be in the past.");
        }

        return ValidationResult.Success!;
    }
}
