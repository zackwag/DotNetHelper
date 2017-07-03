namespace Helper.Extensions
{
    public static class BooleanExtensions
    {
        public static string ToString(this bool value, BooleanText text)
        {
            return value.ToString(text.GetAttributeOfType<BooleanTextAttribute>().TrueValue, text.GetAttributeOfType<BooleanTextAttribute>().FalseValue);
        }

        public static string ToString(this bool value, string trueValue, string falseValue)
        {
            return value ? trueValue : falseValue;
        }
    }
}
