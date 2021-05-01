using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Configuration
{
    public interface IConfigurationOptions
    {
        string ConnectionString { get; init; }
        string Url { get; init; }
        Guid GatewayPaymentId { get; init; }
    }
}
