using System;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DapperHelper.Tools.ExtensionMethods
{
    public static class DataTableExtensionMethods
    {

        public static DataTable GenerateCollumns(this DataTable dataTable, System.Collections.ICollection collection)
        {
            if (collection.GetType().GetGenericArguments().Count() == 1)
            {
                return dataTable.GenerateCollumns(collection.Cast<object>().First());
            }

            throw new ArgumentException($"Error generating Data Table, the model {collection} has multiple data types");

        }

        public static DataTable GenerateCollumns(this DataTable dataTable, object model)
        {
            foreach (var prop in model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }

            return dataTable;
        }

        public static DataTable GenerateRows(this DataTable dataTable, System.Collections.ICollection collection)
        {
            if (dataTable.Columns.Count > 0)
            {
                foreach (var member in collection.Cast<object>())
                {
                    dataTable.GenerateRows(member);

                }

                return dataTable;
            }

            throw new ArgumentException($"Error generating Row Data, the DataTable {dataTable} contains 0 collumns");
        }

        public static DataTable GenerateRows(this DataTable dataTable, object model)
        {
            if (dataTable.Columns.Count > 0)
            {
                dataTable.Rows.Add(model.GetPropertyValues());
                return dataTable;
            }

            throw new ArgumentException($"Error generating Row Data, the DataTable {dataTable} contains 0 collumns");
        }
    }
}
