using DapperHelper.Attributes;
using DapperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DapperHelper.Tools.ExtensionMethods
{
    public static class DbTableExtension
    {
        public static object GetParameterObject<T>(this IDbTableModel tableModel, T attributes) where T : List<Type>
        {
            var parameterObject = new ExpandoObject() as IDictionary<string, object>;

            foreach (Type attribute in attributes)
            {
                if (attribute.GetTypeInfo().BaseType == typeof(Attribute))
                {
                    List<PropertyInfo> propertiesWithAttribute = tableModel.GetType().GetProperties()
                        .Where(x => x.GetCustomAttributes(attribute, true).Any()).ToList();

                    foreach (PropertyInfo proppertyWithAttribute in propertiesWithAttribute)
                    {
                        parameterObject.Add(proppertyWithAttribute.Name, tableModel.GetType().GetProperty(proppertyWithAttribute.Name).GetValue(tableModel));
                    }
                }
            }
            return parameterObject;
        }

        public static object GetParameterObject<T>(this IDbTableModel tableModel, int id, T attributes) where T : List<Type>
        {
            PropertyInfo idPropertyInfo = tableModel.GetType().GetProperties()
                .First(x => x.GetCustomAttributes(typeof(TableIdentityAttribute), true).Any());

            idPropertyInfo.SetValue(tableModel, id);

            return tableModel.GetParameterObject(attributes);
        }

        public static object GetParameterObject<T, U>(this IDbTableModel tableModel, int id, T attributes)
            where T : List<Type>
            where U : IDbTableModel, new()
        {
            PropertyInfo idPropertyInfo = tableModel.GetType().GetProperties()
                .First(x => x.GetCustomAttributes(typeof(TableIdentityAttribute), true).Any());

            idPropertyInfo.SetValue(tableModel, id);

            return tableModel.GetParameterObject(attributes);
        }


    }
}
