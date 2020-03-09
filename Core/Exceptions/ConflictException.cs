using Core.Exceptions.Models;
using System;
using System.Collections.Generic;

namespace Core.Exceptions
{
    public class ConflictException : Exception
    {
        public List<PropertyErrorResult> PropertyErrorResult { get; private set; }
        public ConflictException(List<PropertyErrorResult> propertyErrorResult) : base()
        {
            PropertyErrorResult = propertyErrorResult;
        }
    }
}
