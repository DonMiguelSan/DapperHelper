using System;

namespace DapperHelper.Attributes
{
    /// <summary>
    /// <see cref="Attribute"/> to idenfity a model identity key
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class TableNameAttribute : Attribute
    {

        private readonly string _tableName;

        public string TableName
        {
            get { return _tableName; }
        }


        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }
    }
}
