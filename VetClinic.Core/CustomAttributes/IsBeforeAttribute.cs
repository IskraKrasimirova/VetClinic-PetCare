using System.ComponentModel.DataAnnotations;
using System.Globalization;
using static VetClinic.Common.GlobalConstants.FormattingConstants;

namespace VetClinic.Core.CustomAttributes
{
    public class IsBeforeAttribute : ValidationAttribute
    {
        //private readonly string propertyToCompare;

        //public IsBeforeAttribute(string propertyToCompare, string errorMessage = "")
        //{
        //    this.propertyToCompare = propertyToCompare;
        //    this.ErrorMessage = errorMessage;
        //}

        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    try
        //    {
        //        DateTime dateToCompare = (DateTime)validationContext
        //        .ObjectType
        //        .GetProperty(propertyToCompare)
        //        .GetValue(validationContext.ObjectInstance);

        //        if ((DateTime)value < dateToCompare)
        //        {
        //            return ValidationResult.Success;
        //        }
        //    }
        //    catch (Exception)
        //    { }

        //    return new ValidationResult(ErrorMessage);
        //}

        private readonly DateTime date;

        public IsBeforeAttribute(string dateInput)
        {
            this.date = DateTime.ParseExact(dateInput, NormalDateFormat, CultureInfo.InvariantCulture);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value >= this.date)
            { 
                return new ValidationResult(this.ErrorMessage); 
            }

            return ValidationResult.Success;
        }
    }
}
