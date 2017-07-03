using System.Data;
using System.Data.Common;

namespace Helper.Extensions
{
    public static class DbConnectionExtension
    {
        public static bool IsClosed(this DbConnection value)
        {
            return value.State == ConnectionState.Closed;
        }
    }
}
