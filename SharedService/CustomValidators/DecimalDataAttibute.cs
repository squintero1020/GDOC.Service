using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SharedService.CustomValidators
{
    public class DecimalDataAttibute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string strValue = value as string;
            decimal valueDecimal = 0;
            if (decimal.TryParse(strValue, out valueDecimal))
                return ValidationResult.Success;

            return base.IsValid(value, validationContext);
        }
    }
}
