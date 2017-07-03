using Helper.Enumeration;

namespace Helper.Commerce
{
    public class PaymentTypeAttribute : DefaultEnumAttribute
    {
        public string PaymentSource { get; set; }

        public bool IsCredit { get; set; }
    }
}
