using System;

namespace Helper.Commerce
{
    public class CheckPayment : Payment
    {
        public CheckPayment(string checkNum)
        {
            CheckNum = checkNum;
            Type = PaymentType.Check;
            Contact = null;
            ReferenceNumber = string.Empty;
            Notes = string.Empty;
        }

        public string CheckNum { get; }
    }
}