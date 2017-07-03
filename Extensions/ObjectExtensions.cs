using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Helper.Extensions
{
    public static class ObjectExtensions
    {
        #region 'Is' Methods
        public static bool IsDefault<T>(this T value)
        {
            return EqualityComparer<T>.Default.Equals(value, default(T));
        }

        public static bool IsNullable<T>(this T value)
        {
            var type = typeof(T);
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool IsNull(this object value)
        {
            return value == null || value == DBNull.Value || Convert.IsDBNull(value);
        }

        public static bool HasValue(this object value)
        {
            return !value.IsNull();
        }
        #endregion

        #region 'Get' Methods
        public static Dictionary<string, object> GetPropertyDictionary(this object value)
        {
            var properties = value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return properties.ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(value, null));
        }
        #endregion

        public static bool In<T>(this T value, params T[] values)
        {
            return values.Contains(value);
        }

        public static T Clone<T>(this object value)
        {
            var result = default(T);

            if (value.IsNull()) return result;
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {

                formatter.Serialize(stream, value);
                stream.Seek(0, SeekOrigin.Begin);

                result = (T)formatter.Deserialize(stream);
            }

            return result;
        }

        public static bool TryCast<T>(this object value, out T result)
        {
            result = default(T);

            if (!(value is T)) return false;
            result = (T)value;
            return true;
        }
    }
}
