using System;

namespace Helper.Commerce
{
    public enum PaymentType
    {
        [PaymentType(Name = "Visa", PaymentSource = "VS/MC", IsCredit = true)]
        Visa = 1,
        [PaymentType(Name = "Mastercard", PaymentSource = "VS/MC", IsCredit = true)]
        Mastercard,
        [PaymentType(Name = "Amex", PaymentSource = "AMEX", IsCredit = true)]
        AmericanExpress,
        [PaymentType(Name = "Discover", PaymentSource = "DSCVR", IsCredit = true)]
        Discover,
        [PaymentType(Name = "Free", PaymentSource = "", IsCredit = false)]
        Free,
        [PaymentType(Name = "Check", PaymentSource = "CHECK", IsCredit = false)]
        Check,
        [PaymentType(Name = "No Payment", PaymentSource = "", IsCredit = false)]
        NoPayment,
        [PaymentType(Name = "Transfer", PaymentSource = "TRANSFER", IsCredit = false)]
        Transfer,
        [PaymentType(Name = "Trade", PaymentSource = "TRADE", IsCredit = false)]
        Trade,
        [PaymentType(Name = "Wire", PaymentSource = "WIRE", IsCredit = false)]
        Wire,
    }
}