using Core.Exceptions.Enums;
using Core.Exceptions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestType Type { get; private set; }
        public List<PropertyErrorResult> ValidationResults { get; private set; }

        public BadRequestException() : base() { Type = BadRequestType.General; }

        public BadRequestException(string message) : base(message) { Type = BadRequestType.General; }

        public BadRequestException(List<ValidationResult> validationResult) : base()
        {
            Type = BadRequestType.Validation;
            ValidationResults = MapValidationErrorResult(validationResult);
        }

        public BadRequestException(List<PropertyErrorResult> propertyErrorResults) : base()
        {
            Type = BadRequestType.Validation;
            ValidationResults = propertyErrorResults;
        }

        private List<PropertyErrorResult> MapValidationErrorResult(List<ValidationResult> validationResults)
        {
            var propertyErrorResults = new List<PropertyErrorResult>();

            foreach (var validationResult in validationResults)
            {
                for (var i = 0; i < validationResult.MemberNames.Count(); i++)
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
