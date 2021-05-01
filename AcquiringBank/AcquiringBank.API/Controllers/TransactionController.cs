using AcquiringBank.API.Models.DTO.Requests;
using AcquiringBank.API.Models.DTO.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AcquiringBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<FinancialResponse>> Post([FromBody] PaymentRequest request)
        {
            var context = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();

            if (Validator.TryValidateObject(request, context, validationResults, true))
            {
                return Ok(new FinancialResponse()
                {
                    TransactionId = new Guid(),
                    ErrorDescription = string.Empty,
                    PaymentStatus = Models.Enums.BankPaymentStatus.Accepted
                });
            }

            var message = string.Empty;
            foreach (var validationResult in validationResults)
                message += validationResult.ErrorMessage;

            return new ObjectResult(message)
            {
                StatusCode = StatusCodes.Status405MethodNotAllowed
            };
        }
    }
}
