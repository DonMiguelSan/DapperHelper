using DapperHelper.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DapperHelper.Tools.ExtensionMethods
{
    /// <summary>
    /// Extension methods for data table models 
    /// </summary>
    public static class ObjectExtensionMethods
    {
        /// <summary>
        /// Gets an object containig the parameters dynamically that were marked with the selected
        /// attributes <see cref="Attribute"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="attributes"></param>
        /// <returns>
        /// An <see cref="object"/> containig the parameters marked with <paramref name="attributes"/>
        /// </returns>
        public static object GetParameterObject(this object model, List<Type> attributes = null, Filter filter = Filter.ByValues)
        {
            var parameterObject = new ExpandoObject() as IDictionary<string, object>;

            if (attributes != null)
            {
                foreach (Type attribute in attributes)
                {
                    if (attribute.GetTypeInfo().BaseType == typeof(Attribute))
                    {
                        List<PropertyInfo> propertiesWithAttribute = model.GetType().GetProperties()
                            .Where(x => x.GetCustomAttributes(attribute, true).Any()).ToList();

                        foreach (PropertyInfo proppertyWithAttribute in propertiesWithAttribute)
                        {
                            CustomAttributeData columNameAttrData = proppertyWithAttribute.CustomAttributes?.FirstOrDefault(x => x.AttributeType == typeof(ColumnNameAttribute));

                            if (parameterObject.TryGetValue(proppertyWithAttribute.Name, out _) == false)
                            {
                                var name = columNameAttrData == null ? proppertyWithAttribute.Name : columNameAttrData.ConstructorArguments.FirstOrDefault().Value.ToString().FormatTableName();
                                switch (filter)
                                {
                                    case Filter.ByValues:
                                        parameterObject.Add(name, model.GetType().GetProperty(proppertyWithAttribute.Name).GetValue(model));

                                        break;
                                    case Filter.ByType:
                                        _ = QueryHelpers.NetToSqlTypes.TryGetValue(model.GetType().GetProperty(proppertyWithAttribute.Name).PropertyType.Name, out string propName);
                                        parameterObject.Add(name, propName);
                                        break;

                                }

                            }

                        }
                    }
                }
            }
            else
            {
                var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    CustomAttributeData columNameAttrData = property.CustomAttributes?.FirstOrDefault(x => x.AttributeType == typeof(ColumnNameAttribute));

                    if (parameterObject.TryGetValue(property.Name, out _) == false)
                    {
                        var name = columNameAttrData == null ? property.Name : columNameAttrData.ConstructorArguments.FirstOrDefault().Value.ToString().FormatTableName();

                        switch (filter)
                        {
                            case Filter.ByValues:
                                parameterObject.Add(name, model.GetType().GetProperty(property.Name).GetValue(model));
                                break;
                            case Filter.ByType:
                                _ = QueryHelpers.NetToSqlTypes.TryGetValue(model.GetType().GetProperty(property.Name).PropertyType.Name, out string propName);
                                parameterObject.Add(name, propName);
                                break;

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
        public static object GetParameterObject(this object tableModel, int id, List<Type> attributes)
        {
            PropertyInfo idPropertyInfo = tableModel.GetType().GetProperties()
                .First(x => x.GetCustomAttributes(typeof(KeyIndentityAttribute), true).Any());

            idPropertyInfo.SetValue(tableModel, id);

            return tableModel.GetParameterObject(attributes);
        }

        /// <summary>
        /// Builds a <see cref="string"/> using the properties marked in the namespace <see cref="Attributes"/> using reflection, if
        /// <paramref name="isValueString"/> is set to <see langword="true"/>, the model name will be replace by "Values" and will add an
        /// @ to every marked property in the table model <see cref="IDbTableModel"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="attributes"></param>
        /// <param name="isValueString"></param>
        /// <returns>
        /// A <see cref="string"/>
        /// </returns>
        public static string GetStringForQuery(this object model, List<Type> attributes = null, bool isValueString = false, bool includeDataEncloser = true, bool includeGroupEnclocer = true, string startHeader = "", string startGroupEncloser = "(", string endGroupEncloser = ")", string startDataEncloser = "'", string endDataEncloser = "'", string dataSeparator = ",")
        {
            object filteredAttrObject = model.GetParameterObject(attributes);

            string groupHeader = includeGroupEnclocer ? startGroupEncloser : "";
            string output = $"{startHeader}{groupHeader}";

            var properties = isValueString ? ((IDictionary<string, object>)filteredAttrObject).Values.ToArray()
                : ((IDictionary<string, object>)filteredAttrObject).Keys.ToArray();

            for (int i = 0; i < properties.Count(); i++)
            {
                var valueToBeWritten = properties[i].GetType() == typeof(DateTime) ? ((DateTime)properties[i]).ToString("s") : properties[i];
                valueToBeWritten = includeDataEncloser && valueToBeWritten.ToString().StartsWith(startDataEncloser) == false ? $"{startDataEncloser}{valueToBeWritten}{endDataEncloser}" : valueToBeWritten;
                output += $"{valueToBeWritten}{(i + 1 == properties.Count() ? includeGroupEnclocer ? endGroupEncloser : "" : dataSeparator)}";
            }

            return output;
        }

        public static string GetTypeDefinition(this object model, List<Type> attributes = null)
        {
            object filteredAttrObject = model.GetParameterObject(attributes, Filter.ByType);

            string output = "(";

            var keyPairValues = ((IDictionary<string, object>)filteredAttrObject).ToArray();

            for (int i = 0; i < keyPairValues.Count(); i++)
            {

                string lastCharacter = i == keyPairValues.Count() - 1 ? "" : ",";
                output += $"{QueryHelpers.space}{keyPairValues[i].Key}{QueryHelpers.space}{keyPairValues[i].Value}{lastCharacter}";
            }

            return $"{output})";
        }

        //TODO: Refactor name
        /// <summary>
        /// Generates a <see cref="string"/> to generate a string to be executed as query for diverse operations contained in <see cref="SqlActions"/>
        /// </summary>
        /// <param name="tableModel"></param>
        /// <param name="command"></param>
        /// <returns><
        /// A <see cref="string"/> containig the generated query
        /// /returns>
        public static string GetQueryStringForSqlTrans(this object tableModel, List<Type> parToBeFilttered = null)
        {
            string formattedString = tableModel.GetStringForQuery(parToBeFilttered,
                startHeader: tableModel.GetTableName());
            List<string> queryFiels = new List<string>
            {
                QueryHelpers.cmdInsertInto,
                $"{QueryHelpers.headerDbo}{formattedString}",
                tableModel.GetStringForQuery(parToBeFilttered , true, startGroupEncloser:"Values")
            };

            return queryFiels.FormatQuery();
        }

        public static string FormatQuery(this List<string> queryFields)
        {
            string query = string.Empty;

            for (int i = 0; i < queryFields.Count; i++)
            {
                query = i == 0 ? queryFields[i] : query + QueryHelpers.space + queryFields[i];
            }

            return query;
        }

        public static string GetTableName(this object model)
        {
            object[] customAttr;

            if (model is System.Collections.ICollection dataset && dataset?.Count > 0)
            {
                if (dataset?.Count > 0)
                {
                    customAttr = dataset.Cast<object>().First().GetType().GetCustomAttributes(typeof(TableNameAttribute), true);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                customAttr = model.GetType().GetCustomAttributes(typeof(TableNameAttribute), true);
            }
            if (customAttr.Count() > 0)
            {
                var tableNameAttr = customAttr.FirstOrDefault();

                if (tableNameAttr != null)
                {
                    return (string)tableNameAttr.GetType().GetProperty("TableName").GetValue(tableNameAttr);
                }
            }
            else
            {
                return model.GetType().Name;
            }

            return string.Empty;
        }

        public static string GetColumnName<T>(this object model)
            where T : class
        {
            var property = model.GetType().GetProperties().FirstOrDefault(x => x.GetCustomAttribute(typeof(T)) != null);

            if (property != null)
            {
                return property.Name;
            }
            else
            {
                return "";
            }

        }

        public static DataTable GetDataTable(this object model)
        {
            DataTable output = new DataTable();

            if (model is System.Collections.ICollection dataset)
            {
                if (dataset.Count > 0)
                {
                    output.GenerateCollumns(dataset).GenerateRows(dataset);
                }

                return output;
            }

            output.GenerateCollumns(model).GenerateRows(model);

            return output;
        }


        public static object[] GetPropertyValues(this object model)
        {
            var propertyInfos = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            object[] output = new object[propertyInfos.Count()];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = propertyInfos[i].GetValue(model);
            }

            return output;
        }



    }
}
