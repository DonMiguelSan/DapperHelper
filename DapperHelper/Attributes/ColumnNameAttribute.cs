using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    sealed class ColumNameAttribute : Attribute
    {
        private string _columName;

        public string ColumnName
        {
            get { return _columName; }
            set { _columName = value; }
        }

        public ColumNameAttribute(string columnName)
        {
            _columName = columnName;
        }
    }
}
