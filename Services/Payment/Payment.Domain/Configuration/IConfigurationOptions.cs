using System;

namespace Payment.Domain.Configuration
{
    public interface IConfigurationOptions
    {
        string ConnectionString { get; init; }
        string Url { get; init; }
        Guid GatewayPaymentId { get; init; }
    }
}
