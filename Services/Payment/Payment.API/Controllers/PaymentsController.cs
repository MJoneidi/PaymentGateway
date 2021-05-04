using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
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
        private readonly ICommandHandler<PaymentCommand> _commandHandler;
        private readonly IMapper _mapper;

        public PaymentsController(ILogger<PaymentsController> logger, IPaymentQueries paymentQueries, ICommandHandler<PaymentCommand> commandHandler, IMapper modelMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _paymentQueries = paymentQueries ?? throw new ArgumentNullException(nameof(paymentQueries));
            _mapper = modelMapper ?? throw new ArgumentNullException(nameof(modelMapper));
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
                var paymentRequestCommand = _mapper.Map<PaymentCommand>(paymentRequest);

                _logger.LogInformation($"----- Sending command: paymentRequestCommand, MerchantId:{paymentRequest.MerchantId}, Amount:{paymentRequest.Amount})");

                var commandResult = await _commandHandler.Handle<PaymentCommandResult>(paymentRequestCommand);

                if (commandResult.Status == Domain.Enums.PaymentStatus.Successful)
                    return Ok(commandResult);
                return StatusCode(StatusCodes.Status406NotAcceptable, commandResult.ErrorDescription);
            }
            catch(PaymentApiException ex)
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
