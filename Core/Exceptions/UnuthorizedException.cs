using System;

namespace Core.Exceptions
{
    public class UnuthorizedException : Exception 
    {
        public UnuthorizedException(string message) : base(message) { }
    }
}
