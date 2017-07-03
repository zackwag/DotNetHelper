using Helper.Extensions;

namespace Helper
{
    public class Zipcode
    {
        public Zipcode(string zip)
        {
            Zip5 = zip.Contains("-") ? zip.Split('-')[0] : zip;
            Zip4 = zip.Contains("-") ? zip.Split('-')[1] : string.Empty;
        }

        public Zipcode(string zip5, string zip4)
        {
            Zip5 = zip5;
            Zip4 = zip4;
        }

        public string Zip5 { get; }

        public string Zip4 { get; }

        public override string ToString()
        {
            return Zip4.SafeTrim().Length > 0 ? $"{Zip5}-{Zip4}" : Zip5;
        }
    }
}
