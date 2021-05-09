using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Payment.Domain.DTO.Requests
{
    public class PaymentRequest : IValidatableObject
    {
        [Required]
        public Guid MerchantId { get; set; }

        public Guid GatewayPaymentId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [Required]
        [StringLength(20)]
        public string CardNumber { get; set; }

        [Required]
        public int CardCvv { get; set; }

        [Required]
        [StringLength(5)]
        public string CardExpiry { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (MerchantId == Guid.Empty)
                results.Add(new ValidationResult("Merchant could not be null"));


            if (!IsValidCardNumber(CardNumber))
                results.Add(new ValidationResult("Card number is invalid"));

            if (!IsValidCardExpiry(CardExpiry, out var invalids))
                results.AddRange(invalids.Select(x => new ValidationResult(x)));

            if (Amount <= 0)
                results.Add(new ValidationResult("Amount must be greater than 0"));

            if (Currency == null || !Regex.IsMatch(Currency, "[A-Z]{3}"))
                results.Add(new ValidationResult("Currency is invalid"));


            return results;
        }


        private bool IsValidCardNumber(string cardNumber)
        {
            var regVisaCard = "^4[0-9]{12}(?:[0-9]{3})?$";
            var regMasterCard = "^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$";

            if (string.IsNullOrWhiteSpace(cardNumber) || (!Regex.IsMatch(cardNumber, regVisaCard) && !Regex.IsMatch(cardNumber, regMasterCard)))
                return false;

            return true;
        }

        private bool IsValidCardExpiry(string cardExpiry, out List<string> invalids)
        {
            var reg = "^(0?[1-9]|1[012])/[0-9]{2}$";
            invalids = new List<string>();

            if (string.IsNullOrWhiteSpace(cardExpiry))
                invalids.Add("Card expiry could not be empty");
            if (!Regex.IsMatch(cardExpiry, reg))
                invalids.Add("Card expiry is invalid");

            string[] date = Regex.Split(cardExpiry, "/");
            string[] currentDate = Regex.Split(DateTime.Now.ToString("MM/yy"), "/");
            int compareYears = string.Compare(date[1], currentDate[1]);
            int compareMonths = string.Compare(date[0], currentDate[0]);

            if (compareYears < 0 || (compareYears == 0 && compareMonths < 1))
                invalids.Add("Card has been expired");

            return !invalids.Any();
        }
    }
}
