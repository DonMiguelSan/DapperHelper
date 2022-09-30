
using Dapper;
using DapperHelper.Attributes;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace DapperHelper.Tools
{
    public class DynamicMapper<T> :IDisposable where  T : class, new()
    {
        private readonly Dictionary<string, PropertyInfo> _propertiesMap;
        public DynamicMapper()
        {
            _propertiesMap = new Dictionary<string, PropertyInfo>();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetCustomAttribute(typeof(ColumNameAttribute)) is ColumNameAttribute columNameAttribute)
                {
                    _propertiesMap.Add(columNameAttribute.ColumnName, propertyInfo);
                }
                else
                {
                    _propertiesMap.Add(propertyInfo.Name, propertyInfo);
                }
            }
        }

        public List<T> QueryDynamic(IDbConnection dbConnection, string sqlQuery)
        {
            List<dynamic> results = dbConnection.Query(sqlQuery).ToList();

            List<T> output = new List<T>();

            foreach (dynamic dynObj in results)
            {
                output.Add(AssignPropertyValues(dynObj));
            }

            return output;
        }

        private T AssignPropertyValues(dynamic dynamicObject)
        {
            T output = new T();

            RouteValueDictionary dynamicObjProps = new RouteValueDictionary(dynamicObject);

            foreach (var propName in dynamicObjProps.Keys)
            {
                if (_propertiesMap.TryGetValue(propName, out PropertyInfo propertyMapped)
                    && dynamicObjProps.TryGetValue(propName, out object value))
                {
                    propertyMapped.SetValue(output, value);
                }
            }

            return output;
        }


        public void Dispose()
        {
            _propertiesMap.Clear(); 
        }
    }
}
