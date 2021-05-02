using System;
using System.Text.Json.Serialization;

namespace Payment.Application.Queries
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; init; }      
        public Card Card { get; init; }
        public Money Amount { get; init; }
        public PaymentStatus Status { get; init; }
    }

    public class Card
    {       
        public string MaskedCardNumber { get => getMaskedCardNumber(this.CardNumber); }

        private string CardNumber { get; }
        public string Expiry { get; }

        public Card(string cardNumber, string expiry)
        {
            CardNumber = cardNumber;
            Expiry = expiry;
        }

        private string getMaskedCardNumber(string cardNumber) => $"**** **** **** {cardNumber.Substring(cardNumber.Length - 4)}";
    }
    public class Money
    {
        public string Currency { get; }
        public double Value { get; }

        public Money(double value, string currency)
        {
            Value = value;
            Currency = currency;
        }
    }

    public enum PaymentStatus
    {
        /// <summary>
        ///     Payment was Successful
        /// </summary>
        Successful,

        /// <summary>
        ///     Payment was unsuccessful, might be rejected by bank, processing timeout or processing faulted on `Gateway`
        /// </summary>
        Unsuccessful,
    }
}
