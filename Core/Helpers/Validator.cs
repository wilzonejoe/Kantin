using Core.Exceptions;
using Core.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Helpers
{
    public static class Validator
    {
        public static bool Validate(this IValidationObject validatable)
        {
            var validationContext = new ValidationContext(validatable, null, null);

            var validationResultList = new List<ValidationResult>();
            var validationResult = System.ComponentModel.DataAnnotations.Validator.TryValidateObject
            (
                instance: validatable,
                validationContext: validationContext,
                validationResults: validationResultList,
                validateAllProperties: true
            );

            if (!validationResult && validationResultList.Any())
                throw new BadRequestException(validationResultList);

            return validationResult;
        }
    }
}
