using NUnit.Framework;
using Payment.Domain.Entities;
using System;

namespace Payment.UnitTests.Domain
{
    public class PaymentMethodTests
    {
        private readonly Guid _merchantId = new Guid("d672a444-f1de-4345-e4c3-08d90d74c72a");
        private readonly Guid _acquiringBankId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        private readonly Guid _gatewayPaymentId = new Guid("D333A247-B7F5-48AF-B0EC-08D90CD42263");
        private readonly Guid _paymentResultId = new Guid("00000000-0000-0000-0000-000000000000");

        private readonly double _amount = 1000.0;
        private readonly string _currencyCode = "USD";
        private readonly string _cardNumber = "1234123412341234";
        private readonly string _cardExpiry = "12/22";
        private readonly int _cvv = 245;

        [Test]
        public void Create_PaymentMethod_item_success()
        {
            //Act 
            var fakePaymentMethodItem = new PaymentMethod(_acquiringBankId, _merchantId, _currencyCode, _amount, _cardExpiry, _cvv, _cardNumber);

            //Assert
            Assert.NotNull(fakePaymentMethodItem);
        }

        [Test]
        public void Create_buyer_item_fail()
        {
            //Act - Assert
            Assert.Throws<ArgumentNullException>(() => new PaymentMethod(_acquiringBankId, _merchantId, string.Empty, _amount, _cardExpiry, _cvv, _cardNumber));
        }
    }
}
