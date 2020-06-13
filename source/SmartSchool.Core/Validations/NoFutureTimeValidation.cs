using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartSchool.Core.Validations
{
    public class NoFutureTimeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is DateTime time))
            {
                throw new ArgumentException("Value is not a datetime", nameof(value));
            }

            if (time > DateTime.Now)
            {
                return new ValidationResult("Zeit darf nicht in der Zukunft liegen", new List<string>{"Time"});
            }
            return ValidationResult.Success;
        }
    }
}
