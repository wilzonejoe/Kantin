using System;

namespace Core.Exceptions
{
    public class UnauthorizedException : Exception 
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
