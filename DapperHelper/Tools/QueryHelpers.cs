using System.Collections.Generic;

namespace DapperHelper.Tools
{
    /// <summary>
    /// Class to store string constants to avoid magic strings
    /// </summary>
    public static class QueryHelpers
    {
        /// <summary>
        /// Represents a blank space as a <see cref="string"/> 
        /// </summary>
        public const string space = " ";

        public const string headerDbo = "dbo.";

        public const string cmdInsertInto = "INSERT INTO";

        public const string cmdSelectInto = "SELECT INTO";

        public const string cmdValues = "VALUES";

        public const string cmdSelect = "SELECT";

        public const string cmdSelectAllFrom = "SELECT * FROM";

        public const string cmdFrom = "FROM";

        public const string cmdCreate = "CREATE";

        public const string cmdCreateType = "CREATE TYPE";

        public const string cmdAsTable = "AS TABLE";

        public const string cmdIfExists = "IF EXISTS";

        public const string cmdDrop = "DROP";

        public const string cmdDropTypeIfExists = "DROP TYPE IF EXISTS";

        private static readonly Dictionary<string, string> _netToSqlTypes;
        public static Dictionary<string, string> NetToSqlTypes => _netToSqlTypes;

        static QueryHelpers()
        {
            _netToSqlTypes = new Dictionary<string, string>()
            {
                {"Int64","BigInt"},
                {"Byte[]","VarBinary"},
                {"Byte","TinyInt"},
                {"Boolean","Bit"},
                {"String","nvarchar(max)"},
                {"Char[]","nvarchar(max)"},
                {"DateTime","DateTime2"},
                {"DateTimeOffset","DateTimeOffset"},
                {"Decimal","Money"},
                {"Double","Float"},
                {"Int32","Int"},
                {"Int16","SmallInt"},
                {"Guid","UniqueIdentifier"},
                {"Xml","Xml"},
                {"TimeSpan","Time"}
            };
        }





    }
}
