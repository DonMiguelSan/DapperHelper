using System.Linq;

namespace DapperHelper.Tools.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string FormatTableName(this string tableName)
        {
            if (tableName.Contains(' '))
            {
                return "[" + tableName + "]";
            }

            return tableName;
        }
    }
}
