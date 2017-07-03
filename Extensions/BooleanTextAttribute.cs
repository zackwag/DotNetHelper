using Helper.Enumeration;

namespace Helper.Extensions
{
    public class BooleanTextAttribute : EnumAttribute
    {
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }
    }
}
