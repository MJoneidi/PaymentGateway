using Dapper;
using Payment.Domain.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Payment.Application.Queries
{
    public class PaymentQueries : IPaymentQueries
    {
        private readonly IConfigurationOptions _configurationOptions;
        public PaymentQueries(IConfigurationOptions configurationOptions)
        {
            _configurationOptions = configurationOptions ?? throw new ArgumentNullException(nameof(configurationOptions));
        }


        public async Task<PaymentResponse> GetPaymentAsync(Guid id)
        {
            using (var connection = new SqlConnection(_configurationOptions.ConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<dynamic>(
                   @"select * FROM Payments 
                        WHERE o.Id=@id"
                        , new { id }
                    );

                if (result.AsList().Count == 0)
                    throw new KeyNotFoundException();

                return MapPaymentItem(result);
            }
        }

        private PaymentResponse MapPaymentItem(dynamic result)
        {
            var paymentResponse = new PaymentResponse
            {
                PaymentId = result.PaymentId,
                GatewayPaymentId = result.GatewayPaymentId,
                Card = new Card(result.MaskedCardNumber, result.Expiry),
                Amount = new Money(result.Money, result.Currency),
                Status = result.Status == "1" ? PaymentStatus.Successful : PaymentStatus.Unsuccessful
            };

            return paymentResponse;
        }
    }
}
