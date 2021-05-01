using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Enums
{
    public enum PaymentStatus
    {
        /// <summary>
        ///     Payment was Successful
        /// </summary>
        Successful,

        /// <summary>
        ///     Payment was unsuccessful, might be rejected by bank, processing timeout or processing faulted on `Gateway`
        /// </summary>
        Unsuccessful,
    }
}
