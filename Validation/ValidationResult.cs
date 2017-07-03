namespace Helper.Validation
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string OriginalValue { get; set; }
        public string ProcessedValue { get; set; }
    }
}
