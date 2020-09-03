using System;
using System.Globalization;
using System.Windows.Controls;

namespace Zadatak_1.Validations
{
    class JMBGValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string jmbg = value as string;
            if(jmbg.Length < 13)
            {
                return new ValidationResult(false, "JMBG contains 13 digits");
            }
            else
            {
                DateTime date = DateOfBirth(jmbg);
                if (DateTime.Compare(date.AddYears(18), DateTime.Now) < 0)
                {
                    return new ValidationResult(true, null);
                }
                else
                {
                    return new ValidationResult(false, "Employee can't be younger than 18");
                }
            }
        }
        public DateTime DateOfBirth(string jmbg)
        {
            string day = jmbg.Substring(0, 2);
            int iday = 0;
            Int32.TryParse(day, out iday);
            string month = jmbg.Substring(2, 2);
            int imonth = 0;
            Int32.TryParse(month, out imonth);
            string year = jmbg.Substring(4, 3);
            int iyear = 0;
            Int32.TryParse(year, out iyear);
            if (iyear <= 20)
            {
                DateTime date = new DateTime(iyear + 2000, imonth, iday);
                return date;
            }
            else
            {
                DateTime date = new DateTime(iyear + 1000, imonth, iday);
                return date;
            }
        }
    }
}
