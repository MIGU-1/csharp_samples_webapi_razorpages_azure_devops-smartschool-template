using System;
using System.ComponentModel.DataAnnotations;
using SmartSchool.Core.Entities;

namespace SmartSchool.Persistence.Validations
{
    public class DuplicateSensorValidation : ValidationAttribute
    {
        private readonly UnitOfWork _unitOfWork;

        public DuplicateSensorValidation(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is Sensor sensor))
            {
                throw new ArgumentException("Value is not a sensor", nameof(value));
            }

            if (_unitOfWork.SensorRepository.HasDuplicateAsync(sensor).Result)
            {
                return new ValidationResult("Es existiert bereits ein Sensor mit dem Namen und der Location");
            }
            return ValidationResult.Success;
        }
    }
}
