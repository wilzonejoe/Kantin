using Kantin.Data.Exceptions.Enums;
using Kantin.Data.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kantin.Data.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestType Type { get; private set;} 
        public List<PropertyErrorResult> ValidationResults { get; private set;} 

        public BadRequestException() : base() { Type = BadRequestType.General; }

        public BadRequestException(string message) : base(message) { Type = BadRequestType.General; }

        public BadRequestException(List<ValidationResult> validationResult) : base() 
        {
            Type = BadRequestType.Validation;
            ValidationResults = MapValidationErrorResult(validationResult);
        }

        private List<PropertyErrorResult> MapValidationErrorResult(List<ValidationResult> validationResults)
        {
            var propertyErrorResults = new List<PropertyErrorResult>();

            foreach (var validationResult in validationResults)
            {
                for (var i = 0; i < validationResult.MemberNames.Count(); i++ )
                {
                    var propertyErrorResult = new PropertyErrorResult
                    {
                        FieldName = validationResult.MemberNames.ElementAt(i),
                        FieldErrors = validationResult.ErrorMessage
                    };

                    propertyErrorResults.Add(propertyErrorResult);
                }
            }

            return propertyErrorResults;
        }
    }
}
