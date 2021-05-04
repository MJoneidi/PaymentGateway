using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Exceptions
{
    public class PaymentApiException : Exception
    {
        public PaymentApiException()
        { }

        public PaymentApiException(string message)
            : base(message)
        { }

        public PaymentApiException(string message, Exception innerException)
            : base(message, innerException)
        { 

        }
    }
}
