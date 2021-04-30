using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IPaymentQueries _paymentQueries;
        public PaymentsController(ILogger<PaymentsController> logger, IPaymentQueries paymentQueries)
        {
            _paymentQueries = paymentQueries ?? throw new ArgumentNullException(nameof(paymentQueries));          
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
    }
}
