using System.Text;

namespace Helper.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToString(this byte[] value, Encoding stringEncoding)
        {
            return stringEncoding.GetString(value);
        }
    }
}
