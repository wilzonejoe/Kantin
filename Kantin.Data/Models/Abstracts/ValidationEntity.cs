using Kantin.Data.Exceptions;
using Kantin.Data.Models.Intefaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kantin.Data.Models.Abstracts
{
    public abstract class ValidationEntity : BaseEntity, IValidationObject
    {
        protected ValidationContext ValidationContext { get; private set; }
        public ValidationEntity() : base()
        {
            ValidationContext = new ValidationContext(this, null, null);
        }

        public bool Validate()
        {
            var validationResultList = new List<ValidationResult>();
            var validationResult = Validator.TryValidateObject
            (
                instance: this,
                validationContext: ValidationContext,
                validationResults: validationResultList,
                validateAllProperties: true
            );

            if (!validationResult && validationResultList.Any())
                throw new BadRequestException(validationResultList);

            return validationResult;
        }
    }
}
