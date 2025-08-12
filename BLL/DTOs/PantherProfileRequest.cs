using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BLL.DTOs
{
    public class PantherProfileRequest
    {
        [Required(ErrorMessage = "Panther Type ID is required.")]
        public int PantherTypeId { get; set; }

        [Required(ErrorMessage = "Panther Name is required.")]
        [MinLength(4, ErrorMessage = "Panther Name must be at least 4 characters.")]
        [PantherNameValidation]
        public string PantherName { get; set; } = null!;

        [Required(ErrorMessage = "Weight is required.")]
        [WeightValidation]
        public double Weight { get; set; }
        [Required(ErrorMessage = "Characteristics is required.")]
        public string Characteristics { get; set; } = null!;
        [Required(ErrorMessage = "Warning is required.")]
        public string Warning { get; set; } = null!;
    }

    public class WeightValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not double weight)
                return false;

            if (weight <= 30.0)
            {
                ErrorMessage = "Weight must be greater than 30 kg.";
                return false;
            }

            return true;
        }
    }

    public class PantherNameValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is not string PantherName)
                return false;

            // Check for special characters (#, @, &, (, ))
            if (Regex.IsMatch(PantherName, @"[#@&()]"))
            {
                ErrorMessage = "Panther Name cannot contain special characters (#, @, &, (, )).";
                return false;
            }

            // Check if each word starts with capital letter
            var words = PantherName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                if (word.Length > 0 && !char.IsUpper(word[0]))
                {
                    ErrorMessage = "Each word in Panther Name must start with a capital letter.";
                    return false;
                }

                // Check if word contains only letters after first character
                if (word.Skip(1).Any(c => !char.IsLetter(c)))
                {
                    ErrorMessage = "Panther Name can only contain letters and spaces.";
                    return false;
                }
            }

            return true;
        }
    }
}
