using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
using Payment.Application.Queries;
using Payment.Domain.DTO.Requests;
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


        [Route("{paymentId:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetOrderAsync(Guid paymentId)
        {
            try
            {
                var order = await _paymentQueries.GetPaymentAsync(paymentId);

                return Ok(order);
            }
            catch
            {
                return NotFound();
            }
        }




        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequest paymentRequest)
        {
            var paymentRequestCommand = _mapper.Map<PaymentCommand>(paymentRequest);
            var commandResult = await _commandHandler.Handle(paymentRequestCommand);

            return Ok(commandResult);
        }
    }
}
