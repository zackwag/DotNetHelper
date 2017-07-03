using System.Data.Common;

namespace Helper.Extensions
{
    public static class DbCommandExtensions
    {
        public static bool HasTransaction(this DbCommand value)
        {
            return !value.Transaction.IsNull();
        }
    }
}
