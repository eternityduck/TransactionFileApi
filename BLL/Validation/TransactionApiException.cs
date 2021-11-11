using System;

namespace TestProjectLegioSoft.Validation
{
    public class TransactionApiException : Exception
    {
        public TransactionApiException(string message) : base(message){}
    }
}