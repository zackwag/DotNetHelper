namespace Helper.Lorem
{
    internal class PreceedingDomTextInfo
    {
        public PreceedingDomTextInfo(BoolWrapper isFirstTextOfDocWritten)
        {
            IsFirstTextOfDocWritten = isFirstTextOfDocWritten;
        }
        public bool WritePrecedingWhiteSpace { get; set; }
        public bool LastCharWasSpace { get; set; }
        public readonly BoolWrapper IsFirstTextOfDocWritten;
        public int ListIndex { get; set; }
    }
}
