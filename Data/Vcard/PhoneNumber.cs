using System;

namespace Helper.Data.Vcard
{
    /// <summary>
    /// If you flag the enum types, you may use flags.
    /// </summary>
    public class PhoneNumber
    {
        [Flags]
        public enum PhoneTypes
        {
            None = 0,
            Voice = 1,
            Fax = 2,
            Msg = 4,
            Cell = 8,
            Pager = 16
        }

        public PhoneNumber()
        {
        }

        public PhoneNumber(string number, HomeWorkTypes homeWorkType, PhoneTypes phoneType)
        {
            Number = number;
            HomeWorkType = homeWorkType;
            PhoneType = phoneType;
            Preferred = false;
        }

        public PhoneNumber(string number, HomeWorkTypes homeWorkType, PhoneTypes phoneType, bool preferred)
        {
            Number = number;
            HomeWorkType = homeWorkType;
            PhoneType = phoneType;
            Preferred = preferred;
        }

        public string Number { get; set; }

        public HomeWorkTypes HomeWorkType { get; set; }

        public bool Preferred { get; set; }

        public PhoneTypes PhoneType { get; set; }
    }
}