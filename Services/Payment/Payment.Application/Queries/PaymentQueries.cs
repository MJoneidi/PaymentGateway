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


        public async Task<PaymentResponse> GetPaymentAsync(Guid merchantId, Guid paymentId)
        {
            using (var connection = new SqlConnection(_configurationOptions.ConnectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<dynamic>(
                           @"SELECT [Id]      
                                    ,[MerchantId]
                                    ,[TransactionId]
                                    ,[CurrencyCode]
                                    ,[Amount]
                                    ,[CardExpiry]
                                    ,[CVV]
                                    ,[CardNumber]
                                    ,[Status]      
                             FROM [PaymentsDB].[dbo].[PaymentMethods]
                             WHERE Id=@paymentId AND MerchantId = @merchantId"
                        , new { paymentId, merchantId }
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
                PaymentId = result[0].Id,
                Card = new Card(result[0].CardNumber, result[0].CardExpiry),
                Amount = new Money(result[0].Amount, result[0].CurrencyCode),
                Status = result[0].Status == 0 ? PaymentStatus.Successful : PaymentStatus.Unsuccessful
            };

            return paymentResponse;
        }
    }
}
