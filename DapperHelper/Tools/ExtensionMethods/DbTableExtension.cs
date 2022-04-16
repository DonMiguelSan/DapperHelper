using DapperHelper.Attributes;
using DapperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DapperHelper.Tools.ExtensionMethods
{
    /// <summary>
    /// Extension methods for data table models 
    /// </summary>
    public static class DbTableExtension
    {
        /// <summary>
        /// Gets an object containig the parameters dynamically that were marked with the selected
        /// attributes <see cref="Attribute"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableModel"></param>
        /// <param name="attributes"></param>
        /// <returns>
        /// An <see cref="object"/> containig the parameters marked with <paramref name="attributes"/>
        /// </returns>
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
                        if (parameterObject.TryGetValue(proppertyWithAttribute.Name, out _) == false)
                        {
                            parameterObject.Add(proppertyWithAttribute.Name, tableModel.GetType().GetProperty(proppertyWithAttribute.Name).GetValue(tableModel));
                        }
                    }
                }
            }
            return parameterObject;
        }

        /// <summary>
        /// Geenrates a dynamical <see cref="object"/> containing the <paramref name="attributes"/> extracted from <paramref name="tableModel"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableModel"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <returns>
        /// see cref="object"/> containing the <paramref name="attributes"/> and <paramref name="id"/> as primary identity key
        /// </returns>
        public static object GetParameterObject<T>(this IDbTableModel tableModel, int id, T attributes) where T : List<Type>
        {
            PropertyInfo idPropertyInfo = tableModel.GetType().GetProperties()
                .First(x => x.GetCustomAttributes(typeof(TableIdentityAttribute), true).Any());

            idPropertyInfo.SetValue(tableModel, id);

            return tableModel.GetParameterObject(attributes);
        }

        /// <summary>
        /// Geenrates a dynamical <see cref="object"/> containing the <paramref name="attributes"/> extracted from <paramref name="tableModel"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableModel"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <returns>
        /// see cref="object"/> containing the <paramref name="attributes"/> and <paramref name="id"/> as primary identity key
        /// </returns>
        public static object GetParameterObject<T, U>(this U tableModel, int id, T attributes)
            where T : List<Type>
            where U : IDbTableModel, new()
        {
            PropertyInfo idPropertyInfo = tableModel.GetType().GetProperties()
                .First(x => x.GetCustomAttributes(typeof(TableIdentityAttribute), true).Any());

            idPropertyInfo.SetValue(tableModel, id);

            return tableModel.GetParameterObject(attributes);
        }

        public static string GetStringForQuery<T>(this IDbTableModel tableModel, T attributes, bool isValueString = false)
            where T : List<Type>
        {
            object filteredAttrObject = tableModel.GetParameterObject(attributes);

            string output = isValueString?"Values(": $"{nameof(tableModel)}(";

            var properties = ((IDictionary<string, object>)filteredAttrObject).Keys.ToArray();

            for (int i = 0; i < properties.Count(); i++) 
            {
                output += $"{properties[i]}{(i + 1 == properties.Count() ? ")" : ", ")}";
            }

            return output;
        }

    }
}
