using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Zadatak_1.Validations
{
    class IDCardValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string number = value as string;
            if (number.Length != 9 || !number.All(char.IsDigit))
            {
                return new ValidationResult(false, "ID Card contains 9 digits");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }

}
