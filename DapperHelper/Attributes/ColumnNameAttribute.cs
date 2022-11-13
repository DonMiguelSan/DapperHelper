using System;

namespace DapperHelper.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class ColumnNameAttribute : Attribute
    {
        private string _columName;

        public string ColumnName
        {
            get { return _columName; }
            set { _columName = value; }
        }

        public ColumnNameAttribute(string columnName)
        {
            _columName = columnName;
        }
    }
}
