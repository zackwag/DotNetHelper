using System;

namespace Helper.Data.Vcard
{
    public class Url
    {
        public Url()
        { }

        public Url(string address, HomeWorkTypes homeWorkType)
        {
            Address = address;
            HomeWorkType = homeWorkType;
        }

        public string Address { get; set; }

        public HomeWorkTypes HomeWorkType { get; set; }
    }
}