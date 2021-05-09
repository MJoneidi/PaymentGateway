using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
using Payment.Application.Processors.Contracts;
using Payment.Application.Queries;
using Payment.Domain.DTO.Requests;
using Payment.Domain.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentQueries _paymentQueries;
        private readonly IPaymentProcessor _paymentProcessor;
       

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentQueries paymentQueries, IPaymentProcessor paymentProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _paymentProcessor = paymentProcessor ?? throw new ArgumentNullException(nameof(paymentProcessor));
            _paymentQueries = paymentQueries ?? throw new ArgumentNullException(nameof(paymentQueries));           
        }


        [HttpGet("details")]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Get(Guid merchantId, Guid paymentId)
        {
            try
            {
                _logger.LogInformation($"----- Get payment details: paymentId, MerchantId:{merchantId}, PaymentId:{paymentId}");
                var order = await _paymentQueries.GetPaymentAsync(merchantId, paymentId);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return StatusCode(StatusCodes.Status404NotFound, "PaymentId not found");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest paymentRequest)
        {
            try
            {     
                _logger.LogInformation($"----- Getting payment request : MerchantId:{paymentRequest.MerchantId}, Amount:{paymentRequest.Amount})");

                var response = await _paymentProcessor.ProcessAsync(paymentRequest);

                return Ok(response);   
            }
            catch (PaymentApiException ex)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, ex.Message); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Error");
        }
    }
}
