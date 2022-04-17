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

        /// <summary>
        /// Builds a <see cref="string"/> using the properties marked in the namespace <see cref="Attributes"/> using reflection, if
        /// <paramref name="isValueString"/> is set to <see langword="true"/>, the model name will be replace by "Values" and will add an
        /// @ to every marked property in the table model <see cref="IDbTableModel"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableModel"></param>
        /// <param name="attributes"></param>
        /// <param name="isValueString"></param>
        /// <returns>
        /// A <see cref="string"/>
        /// </returns>
        public static string GetStringForQuery<T>(this IDbTableModel tableModel, T attributes, bool isValueString = false)
            where T : List<Type>
        {
            object filteredAttrObject = tableModel.GetParameterObject(attributes);

            string output = isValueString?"Values(": $"{tableModel.GetType().Name}(";

            var properties = ((IDictionary<string, object>)filteredAttrObject).Keys.ToArray();

            for (int i = 0; i < properties.Count(); i++) 
            {
                output += $"{(isValueString ? "@" : "")}{properties[i]}{(i + 1 == properties.Count() ? ")" : ", ")}";
            }

            return output;
        }

        /// <summary>
        /// Builds a <see cref="string"/> using the properties marked in the namespace <see cref="Attributes"/> using reflection, if
        /// <paramref name="isValueString"/> is set to <see langword="true"/>, the model name will be replace by "Values" and will add an
        /// @ to every marked property in the table model <see cref="IDbTableModel"/>
        /// </summary>
        /// <param name="tableModel"></param>
        /// <param name="isValueString"></param>
        /// <returns>
        /// A <see cref="string"/>
        /// </returns>
        public static string GetQueryStringWithUpdatablePar(this IDbTableModel tableModel, bool isValueString = false)
        {
            return tableModel.GetStringForQuery(new List<Type>() { typeof(UpdatableParAttribute) }, isValueString);

        }
        /// <summary>
        /// Generates a <see cref="string"/> to generate a string to be executed as query for diverse operations contained in <see cref="SqlCommands"/>
        /// </summary>
        /// <param name="tableModel"></param>
        /// <param name="command"></param>
        /// <returns><
        /// A <see cref="string"/> containig the generated query
        /// /returns>
        public static string GetQueryStringForSqlTrans(this IDbTableModel tableModel, SqlCommands command)
        {
            return $"{command}{StringConstants.blankSpace}{tableModel.GetQueryStringWithUpdatablePar()}" +
                         $"{StringConstants.blankSpace}{tableModel.GetQueryStringWithUpdatablePar(true)}";
        }
    }
}
