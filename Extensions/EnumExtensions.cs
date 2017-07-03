using Helper.Enumeration;
using System;
using System.Linq;

namespace Helper.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttributeOfType<T>(this Enum value) where T : EnumAttribute
        {
            var memInfo = value.GetType().GetMember(value.ToString());
            var attributes = memInfo.First().GetCustomAttributes(typeof(T), false);

            return attributes.HasItems() ? (T)attributes.First() : null;
        }
    }
}
