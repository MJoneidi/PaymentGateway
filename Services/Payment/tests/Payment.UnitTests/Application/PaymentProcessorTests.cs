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
using AutoMapper;
using Payment.Application.Processors;
using Payment.Domain.DTO.Requests;

namespace Payment.UnitTests.Application
{
    public class PaymentProcessorTests
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
        private Mock<IMapper> _mapper;
        private Mock<ILogger<PaymentProcessor>> _logger;
        private Mock<ICommandHandler<CreatePaymentCommand>> _commandHandler;
       

        [SetUp]
        public void Setup()
        {
            _acquiringBankAdapter = new Mock<IAcquiringBankAdapter>();
            _commandHandler = new Mock<ICommandHandler<CreatePaymentCommand>>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<PaymentProcessor>>();
        }


        [Test]
        public void PaymentCommandHandler_ValidRequest_Success()
        {
            // Arrange
            var request = FakeRequest();
            var bankResponse = FakeFinancialResponse();
            var paymentResponse = FakePaymentResponse();
            var commandRequest = FakeCommandRequest();
            var paymentProcessor = new PaymentProcessor(_commandHandler.Object, _mapper.Object ,_logger.Object,_acquiringBankAdapter.Object);
           

            _acquiringBankAdapter.Setup(x => x.SendRequestAsync(request)).ReturnsAsync(bankResponse);
            _mapper.Setup(x => x.Map<CreatePaymentCommand>(It.IsAny<PaymentRequest>())).Returns(commandRequest);
           

            // Act
            var result = paymentProcessor.ProcessAsync(request).Result;


            //Assert
            Assert.AreEqual(result.Status, paymentResponse.Status);
            _acquiringBankAdapter.Verify(x => x.SendRequestAsync(request), Times.Once);
            _commandHandler.Verify(x => x.Handle(commandRequest), Times.Once);
        }

        private PaymentResponse FakePaymentResponse() => new PaymentResponse()
        {
            Status = PaymentStatus.Successful,
            PaymentResultId = _paymentResultId
        };
        private FinancialResponse FakeFinancialResponse() => new FinancialResponse()
        {
            PaymentStatus = PaymentStatus.Successful,
            TransactionId = _transactionId
        };

        private PaymentRequest FakeRequest() => new PaymentRequest()
        {
            Amount = _amount,
            CardCvv = _cvv,
            CardExpiry = _cardExpiry,
            CardNumber = _cardNumber,
            Currency = _currencyCode,
            GatewayPaymentId = _gatewayPaymentId,
            MerchantId = _merchantIdValid            
        };

        private CreatePaymentCommand FakeCommandRequest() => new CreatePaymentCommand()
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

