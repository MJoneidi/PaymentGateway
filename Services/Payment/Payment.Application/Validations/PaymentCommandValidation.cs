using FluentValidation;
using Microsoft.Extensions.Logging;
using Payment.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Validations
{
    public class PaymentCommandValidation : AbstractValidator<PaymentCommand>
    {
        public PaymentCommandValidation()
        {
            RuleFor(command => command.Amount).NotEmpty();
            RuleFor(command => command.Currency).NotEmpty().Length(3).Must(BeValidCurrency).WithMessage("Please specify a valid currency value");
            RuleFor(command => command.CardNumber).NotEmpty().Length(16, 20);
            RuleFor(command => command.CardCvv).NotEmpty();
            RuleFor(command => command.CardExpiry).NotEmpty().Must(BeValidExpirationDate).WithMessage("Please specify a valid card expiration date");            
            RuleFor(command => command.MerchantId).NotEmpty().Must(BeValidMerchant).WithMessage("Please specify a valid merchant"); 
        }

        private bool BeValidExpirationDate(string dateTime)
        {
            return true;
        }

        private bool BeValidCurrency(string currency)
        {
            // it should load first on startup into config list in singleton scope, and then use it here instead of hardcode 
            List<string> validCurrencies = new List<string>() { "USD", "EUR" };

            return validCurrencies.Contains(currency);
        }

        private bool BeValidMerchant(Guid merchantId)
        {
            // it should load first on startup into config list in singleton scope, and then use it here instead of hardcode 
            Dictionary<string, Guid> validMerchants = new Dictionary<string, Guid>() {{ "Amazon", new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6") } };

            return validMerchants.Any(rec=> rec.Value == merchantId);
        }
    }
}
