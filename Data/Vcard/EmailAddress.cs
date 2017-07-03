using System;

namespace Helper.Data.Vcard
{
    public class EmailAddress
    {
        public EmailAddress(string address)
        {
            Address = address;
            Preferred = false;
        }

        public EmailAddress(string address, bool preferred)
        {
            Address = address;
            Preferred = preferred;
        }

        public string Address { get; set; }

        public bool Preferred { get; set; }
    }
}