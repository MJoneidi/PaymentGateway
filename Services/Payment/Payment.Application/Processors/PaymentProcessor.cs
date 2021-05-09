using AutoMapper;
using Microsoft.Extensions.Logging;
using Payment.Application.BankAdaptors.Contracts;
using Payment.Application.Commands;
using Payment.Application.Commands.Contracts;
using Payment.Application.Processors.Contracts;
using Payment.Application.Validations;
using Payment.Domain.DTO.Requests;
using Payment.Domain.DTO.Response;
using Payment.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Processors
{
    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly ICommandHandler<CreatePaymentCommand> _commandHandler;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentProcessor> _logger;
        private readonly IAcquiringBankAdapter _acquiringBankAdapter;

        public PaymentProcessor(ICommandHandler<CreatePaymentCommand> commandHandler, IMapper modelMapper, ILogger<PaymentProcessor> logger, IAcquiringBankAdapter acquiringBankAdapter)
        {
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _mapper = modelMapper ?? throw new ArgumentNullException(nameof(modelMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _acquiringBankAdapter = acquiringBankAdapter ?? throw new ArgumentNullException(nameof(acquiringBankAdapter));
        }


        public async Task<PaymentResponse> ProcessAsync(PaymentRequest request)
        {         
            var validator = new PaymentValidation();
            var failures = validator.Validate(request);

            if (!failures.IsValid)
            {
                _logger.LogWarning("Validation errors - PaymentProcessor : {@request} - Errors: {@ValidationErrors}", request, failures.Errors);

                throw new PaymentApiException(string.Join(",", failures.Errors));
            }

            var response = await _acquiringBankAdapter.SendRequestAsync(request);

            var command = _mapper.Map<CreatePaymentCommand>(request);

            command.TransactionId = response.TransactionId;
            command.PaymentStatus = response.PaymentStatus;
            command.ErrorDescription = response.ErrorDescription;

            await _commandHandler.Handle(command);

            return new PaymentResponse()
            {
                Status = response.PaymentStatus,
                PaymentResultId = command.Id,
                ErrorDescription = response.ErrorDescription
            } ;           
        }
    }
}
