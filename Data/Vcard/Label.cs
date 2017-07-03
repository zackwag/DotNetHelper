namespace Helper.Data.Vcard
{
    /// <summary>
    /// Not used yet. You may use regular expressions or String.Replace() to replace =0D=0A to line breaks.
    /// </summary>
    public class Label
    {
        public enum LabelTypes
        {
            Dom,
            Intl,
            Postal,
            Parcel
        }

        public string Address { get; set; }

        public LabelTypes LabelType { get; set; }
    }
}