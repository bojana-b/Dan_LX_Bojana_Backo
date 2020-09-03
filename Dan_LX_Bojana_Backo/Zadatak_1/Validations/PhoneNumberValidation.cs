using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Zadatak_1.Validations
{
    class PhoneNumberValidation : ValidationRule
    {
        /// <summary>
        /// This method checks if a forwarded phone number in specific format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string phoneNumber = value as string;
            //validation for Serbian numbers
            if (Regex.Match(phoneNumber, @"^(\+3816[0-9]{7})$").Success || Regex.Match(phoneNumber, @"^(\+3816[0-9]{8})$").Success)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Format +3816...");
            }
        }
    }
}
