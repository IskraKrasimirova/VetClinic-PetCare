using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static VetClinic.Common.DefaultHourSchedule;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Core.CustomAttributes
{
    public class ValidateHourStringAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var hourAsString = value as string;

            if (string.IsNullOrEmpty(hourAsString))
            {
                return false;
            }

            var cultureInfo = CultureInfo.GetCultureInfo("bg-BG");

            bool isParsed = DateTime.TryParseExact(hourAsString, HourFormat, cultureInfo, DateTimeStyles.None, out _);

            if (!isParsed)
            {
                return false;
            }

            if (HourScheduleAsString == null)
                SeedHourScheduleAsString();

            if (!HourScheduleAsString.Contains(hourAsString.Trim()))
            {
                return false;
            }

            return true;
        }
    }
}
