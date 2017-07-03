namespace Helper.Lorem
{
    public enum ParagraphLength
    {
        [ServiceParameter(Parameter = "")]
        NotSpecified = 0,
        [ServiceParameter(Parameter = "short")]
        Short,
        [ServiceParameter(Parameter = "medium")]
        Medium,
        [ServiceParameter(Parameter = "long")]
        Long,
        [ServiceParameter(Parameter = "verylong")]
        VeryLong
    }
}
