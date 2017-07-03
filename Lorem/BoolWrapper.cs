namespace Helper.Lorem
{
    internal class BoolWrapper
    {
        public bool Value { get; set; }

        public static implicit operator bool (BoolWrapper boolWrapper)
        {
            return boolWrapper.Value;
        }

        public static implicit operator BoolWrapper(bool boolWrapper)
        {
            return new BoolWrapper { Value = boolWrapper };
        }
    }
}
