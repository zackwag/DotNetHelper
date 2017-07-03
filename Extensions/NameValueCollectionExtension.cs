using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Helper.Extensions
{
    public static class NameValueCollectionExtension
    {
        public static bool ContainsKey(this NameValueCollection value, string key)
        {
            if (!value.Get(key).IsNull()) return true;
            return value.AllKeys.Contains(key);
        }

        public static T GetValue<T>(this NameValueCollection value, string key, T defaultValue)
        {
            if (!value.HasItems()) return defaultValue;
            if (!string.IsNullOrEmpty(key) && value[key].HasValue())
                return (T)Convert.ChangeType(value[key], typeof(T));

            return defaultValue;
        }

        public static IEnumerable<KeyValuePair<string, string>> ToPairs(this NameValueCollection value)
        {
            return value.Cast<string>().Select(key => new KeyValuePair<string, string>(key, value[key]));
        }

        public static ILookup<string, string> ToLookup(this NameValueCollection value)
        {
            return (from key in value.Cast<string>()
                    from val in value.GetValues(key)
                    select new { key, val }).ToLookup(p => p.key, p => p.val);
        }
    }
}
