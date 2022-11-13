
using Dapper;
using DapperHelper.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Routing;

namespace DapperHelper.Tools
{
    public class DynamicMapper<T> : IDisposable where T : class, new()
    {
        private Dictionary<string, PropertyInfo> _propertiesMap;

        private IEnumerable<dynamic> queryResult;

        private readonly IDbConnection dbConnection;

        public DynamicMapper()
        {
            GeneratePropsMap();
        }
        public DynamicMapper(string conStringName) : this()
        {
            dbConnection = new SqlConnection(ConfigManagerHelper.GetConnectionString(conStringName));
        }

        public List<T> QueryDynamic(string sqlQuery, int? Timeout = 1000, IDbConnection dbConnection = null)
        {
            var con = dbConnection ?? this.dbConnection;
            try
            {
                con.Open();

                queryResult = con.Query(sqlQuery, commandTimeout: Timeout);

                con.Close();

            }
            catch (Exception)
            {
                con?.Close();
            }

            return MapResult();
        }

        public async Task<List<T>> QueryDynamicAsync(string sqlQuery, int? Timeout = 1000, IDbConnection dbConnection = null)
        {
            var con = dbConnection ?? this.dbConnection;

            try
            {
                con.Open();

                queryResult = await con.QueryAsync(sqlQuery, commandTimeout: Timeout);

                con.Close();
            }
            catch (Exception)
            {
                con.Close();
            }

            return MapResult();
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

        private void GeneratePropsMap()
        {
            _propertiesMap = new Dictionary<string, PropertyInfo>();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.GetCustomAttribute(typeof(ColumnNameAttribute)) is ColumnNameAttribute columNameAttribute)
                {
                    _propertiesMap.Add(columNameAttribute.ColumnName, propertyInfo);
                }
                else
                {
                    _propertiesMap.Add(propertyInfo.Name, propertyInfo);
                }
            }
        }

        private List<T> MapResult()
        {
            List<T> output = new List<T>();

            foreach (dynamic dynObj in queryResult.ToList())
            {
                output.Add(AssignPropertyValues(dynObj));
            }

            return output;
        }

        public void Dispose()
        {
            _propertiesMap.Clear();

            dbConnection?.Close();
        }
    }
}
