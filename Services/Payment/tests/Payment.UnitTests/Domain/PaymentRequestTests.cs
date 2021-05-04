using NUnit.Framework;
using Payment.Domain.DTO.Requests;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Payment.UnitTests.Domain
{
    public class PaymentRequestTests
    {
        [Test]
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 100, "USD", "5555555555554344", 123, "12/99")]
        public void Pass_validation_paymentrequest_success(Guid merchantId, double amount, string currency, string cardNumber, int cardCvv, string cardExpiry)
        {
            // Arrange
            var fakePaymentMethodItem = new PaymentRequest()
            {
                Amount = amount,
                CardCvv = cardCvv,
                CardExpiry = cardExpiry,
                CardNumber = cardNumber,
                Currency = currency,
                MerchantId = merchantId
            };
            var context = new ValidationContext(fakePaymentMethodItem);

            //Act 
            var validationResults = fakePaymentMethodItem.Validate(context);

            //Assert
            Assert.AreEqual(validationResults.Any(), false);
        }

        [Test]
        [TestCase("00000000-0000-0000-0000-000000000000", 100, "USD", "5555555555554344", 123, "12/99")] // invalid merchantId 
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 0, "USD", "5555555555554344", 123, "12/99")]   // invalid amount
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 100, "US", "5555555555554344", 123, "12/99")]  // invalid currency
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 100, "USD", "6655555555554344", 123, "12/99")] // invalid cardNumber
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 100, "USD", "55555555554344", 123, "12/99")]   // invalid cardNumber
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 100, "USD", "", 123, "12/02")]                 // card is expired
        [TestCase("d672a444-f1de-4345-e4c3-08d90d74c72a", 100, "USD", "", 123, "12002")]                 // invalid cardExpiry
        public void Pass_validation_paymentrequest_fail(Guid merchantId, double amount, string currency, string cardNumber, int cardCvv, string cardExpiry)
        {
            var fakePaymentMethodItem = new PaymentRequest()
            {
                Amount = amount,
                CardCvv = cardCvv,
                CardExpiry = cardExpiry,
                CardNumber = cardNumber,
                Currency = currency,
                MerchantId = merchantId
            };
            var context = new ValidationContext(fakePaymentMethodItem);

            //Act 
            var validationResults = fakePaymentMethodItem.Validate(context);

            //Assert
            Assert.AreEqual(validationResults.Any(), true);
        }
    }
}
