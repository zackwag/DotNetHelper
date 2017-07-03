using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;

namespace Helper.Extensions
{
    public static class MailAddressExtensions
    {
        public static EmailAddress ToEmailAddress(this MailAddress value)
        {
            return string.IsNullOrEmpty(value.DisplayName) ? new EmailAddress(value.Address) : new EmailAddress(value.DisplayName, value.Address);
        }
    }
}
