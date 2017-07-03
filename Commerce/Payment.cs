using System;
using Helper.Extensions;

namespace Helper.Commerce
{
    public class Payment
    {
        public PaymentType Type;
        public string ReferenceNumber;
        public string Notes;
        public Contact Contact;

        public bool IsCredit => Type == PaymentType.AmericanExpress || Type == PaymentType.Discover || Type == PaymentType.Mastercard || Type == PaymentType.Visa;

        public bool IsCheck => Type == PaymentType.Check;

        public bool IsTransfer => Type == PaymentType.Transfer;

        public bool IsTrade => Type == PaymentType.Trade;

        public bool IsWire => Type == PaymentType.Wire;

        public string PaymentSource => Type.GetAttributeOfType<PaymentTypeAttribute>().PaymentSource;
    }
}