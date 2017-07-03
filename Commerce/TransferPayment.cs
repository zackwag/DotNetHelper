using System;

namespace Helper.Commerce
{
    public class TransferPayment : Payment
    {
        public TransferPayment(string referenceNumber, string notes)
        {
            ReferenceNumber = referenceNumber;
            Notes = notes;
            Type = PaymentType.Transfer;
            Contact = null;
        }
    }
}