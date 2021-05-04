using Moq;
using NUnit.Framework;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
using Payment.Application.BankAdaptors.Contracts;
using Payment.Domain.DTO.Response;
using Payment.Domain.Entities;
using Payment.Domain.Enums;
using Payment.Infrastructure.Data.Repositories.Contracts;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Payment.Application.Validations;

namespace Payment.UnitTests.Application
{
    public class PaymentCommandHandlerTests
    {
        private readonly Guid _transactionId = new Guid("d672a444-f1de-4345-e4c3-08d90d74c72a");
        private readonly Guid _merchantIdValid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        private readonly Guid _merchantIdInvalid = new Guid("3fa85f64-5717-4562-b3fc-2c963f66af44");
        private readonly Guid _gatewayPaymentId = new Guid("D333A247-B7F5-48AF-B0EC-08D90CD42263");
        private readonly Guid _paymentResultId = new Guid("00000000-0000-0000-0000-000000000000");

        private readonly double _amount = 1000.0;
        private readonly string _currencyCode = "USD";
        private readonly string _cardNumber = "1234123412341234";
        private readonly string _cardExpiry = "12/22";
        private readonly int _cvv = 245;

        private Mock<IAcquiringBankAdapter> _acquiringBankAdapter;
        private Mock<IPaymentMethodRepository> _paymentMethodRepository;
        private Mock<ILogger<PaymentCommandHandler>> _logger;

        [SetUp]
        public void Setup()
        {
            _acquiringBankAdapter = new Mock<IAcquiringBankAdapter>();
            _paymentMethodRepository = new Mock<IPaymentMethodRepository>();
            _logger = new Mock<ILogger<PaymentCommandHandler>>();
        }


        [Test]
        public void PaymentCommandHandler_ValidRequest_Success()
        {
            // Arrange
            var request = FakeCommandRequest();
            var bankResponse = FakeFinancialResponse();
            var paymentCommandResult = FakePaymentCommandResult();
            var handler = new PaymentCommandHandler(_logger.Object,_acquiringBankAdapter.Object, _paymentMethodRepository.Object);
           

            _acquiringBankAdapter.Setup(x => x.SendRequestAsync(request)).ReturnsAsync(bankResponse);

            _paymentMethodRepository.Setup(x => x.Add(
                    It.Is<PaymentMethod>(
                                    p => p.Amount == _amount &&
                                    p.CurrencyCode == _currencyCode &&
                                    p.AcquiringBankId == _merchantIdValid &&
                                    p.CardExpiry == _cardExpiry &&
                                    p.CardNumber == _cardNumber &&
                                    p.CVV == _cvv
                    )))
                    .Returns(Task.CompletedTask);

            // Act
            var result = handler.Handle<PaymentCommandResult>(request).Result;


            //Assert
            Assert.AreEqual(result.Status, paymentCommandResult.Status);
            _acquiringBankAdapter.Verify(x => x.SendRequestAsync(request), Times.Once);
        }

        private PaymentCommandResult FakePaymentCommandResult() => new PaymentCommandResult()
        {
            Status = PaymentStatus.Successful,
            PaymentResultId = _paymentResultId
        };
        private FinancialResponse FakeFinancialResponse() => new FinancialResponse()
        {
            PaymentStatus = PaymentStatus.Successful,
            TransactionId = _transactionId
        };

        private PaymentCommand FakeCommandRequest() => new PaymentCommand()
        {
            Amount = _amount,
            CardCvv = _cvv,
            CardExpiry = _cardExpiry,
            CardNumber = _cardNumber,
            Currency = _currencyCode,
            GatewayPaymentId = _gatewayPaymentId,
            MerchantId = _merchantIdValid
        };
    }
}

