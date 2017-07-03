using System;

namespace Helper.Data.Vcard
{
    public class Address
    {
        private string _ext;
        private string _po;
        private string _street;
        private string _region;
        private string _locality;
        private string _postcode;
        private string _country;
        private readonly char[] _lineBreak = { '\n', '\r' };

        public string PoBox
        {
            get
            {
                return _po;
            }
            set
            {
                _po = value.TrimEnd(_lineBreak);
            }
        }

        public string Ext
        {
            get
            {
                return _ext;
            }
            set
            {
                _ext = value.TrimEnd(_lineBreak);
            }
        }

        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value.TrimEnd(_lineBreak);
            }
        }

        public string Locality
        {
            get
            {
                return _locality;
            }
            set
            {
                _locality = value.TrimEnd(_lineBreak);
            }
        }

        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                _region = value.TrimEnd(_lineBreak);
            }
        }

        public string Postcode
        {
            get
            {
                return _postcode;
            }
            set
            {
                _postcode = value.TrimEnd(_lineBreak);
            }
        }

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value.TrimEnd(_lineBreak);
            }
        }

        public HomeWorkTypes HomeWorkType { get; set; }
    }
}