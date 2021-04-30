using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Queries
{
    public class PaymentQueries : IPaymentQueries
    {
        private string _connectionString = string.Empty;

        public PaymentQueries(string connectionString)
        {
            _connectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
        }


        public async Task<PaymentResponse> GetPaymentAsync(Guid id)
        {
            using (var connection = new SqlConnection(_connectionString))
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
                Status = result.Status == "1"? PaymentStatus.Successful : PaymentStatus.Unsuccessful
            };           

            return paymentResponse;
        }
    }
}
