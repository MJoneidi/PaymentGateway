using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Payment.Application.BankAdaptors;
using Payment.Application.Commands;
using Payment.Domain.Configuration;
using Payment.Domain.DTO.Response;
using Payment.Domain.Enums;
using System;

namespace Payment.UnitTests.Application
{
    public class AcquiringBankAdapterTests
    {
        private readonly string _connectionString = "Server=.;Database=PaymentsDB;User Id=sa;Password=Pass;";
        private readonly string _url = "http://acquiringbank.api:7002/api/Transaction";

        private readonly Guid _merchantId = new Guid("d672a444-f1de-4345-e4c3-08d90d74c72a");
        private readonly Guid _transactionId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        private readonly Guid _gatewayPaymentId = new Guid("D333A247-B7F5-48AF-B0EC-08D90CD42263");
        private readonly Guid _paymentResultId = new Guid("00000000-0000-0000-0000-000000000000");

        private readonly double _amount = 1000.0;
        private readonly string _currencyCode = "USD";
        private readonly string _cardNumber = "1234123412341234";
        private readonly string _cardExpiry = "12/22";
        private readonly int _cvv = 245;

        private Mock<ILogger<AcquiringBankAdapter>> _logger;
        private Mock<IConfigurationOptions> _configurationOptions;


        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<AcquiringBankAdapter>>();
            _configurationOptions = new Mock<IConfigurationOptions>();
        }


        //[Test]
        //public void AcquiringBankAdapter_ValidRequest_Success()
        //{
        //    // Arrange
        //    var request = FakeCommandRequest();
        //    var bankResponse = FakeFinancialResponse();

        //    var adapter = new AcquiringBankAdapter(_logger.Object, _configurationOptions.Object);

        //    _configurationOptions.Setup(x => x.GatewayPaymentId).Returns(_gatewayPaymentId);
        //    _configurationOptions.Setup(x => x.ConnectionString).Returns(_connectionString);
        //    _configurationOptions.Setup(x => x.Url).Returns(_url);


        //    var mockHandler = new Mock<HttpMessageHandler>();
        //    mockHandler.Protected()
        //               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
        //            ItExpr.IsAny<CancellationToken>())
        //               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        //    // Act
        //    var result = adapter.SendRequestAsync(request).Result;

        //    //Assert
        //    Assert.AreEqual(result.PaymentStatus, PaymentStatus.Successful);            
        //}

        [Test]
        public void AcquiringBankAdapter_handle_exception_when_no_url()
        {
            //Assert
            var adapter = new AcquiringBankAdapter(_logger.Object, _configurationOptions.Object);

            _configurationOptions.Setup(x => x.GatewayPaymentId).Returns(_gatewayPaymentId);
            _configurationOptions.Setup(x => x.ConnectionString).Returns(_connectionString);
            var request = FakeCommandRequest();
            var bankResponse = FakeFinancialResponse();

            // Act
            var result = adapter.SendRequestAsync(request).Result;

            //Assert
            Assert.AreEqual(result.PaymentStatus, PaymentStatus.Unsuccessful);
            _logger.Verify(x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }




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
            MerchantId = _merchantId
        };
    }
}

