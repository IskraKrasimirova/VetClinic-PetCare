using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Core.CustomAttributes
{
    public class ValidateDateStringAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateAsString = value as string;

            if (string.IsNullOrEmpty(dateAsString))
            {
                return false;
            }

            bool isParsed = DateTime.TryParseExact(
                dateAsString,
                DateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime dateTime);

            if (!isParsed)
            {
                return false;
            }

            if (dateTime < DateTime.UtcNow.Date)
            {
                return false;
            }

            return true;
        }
    }
}
