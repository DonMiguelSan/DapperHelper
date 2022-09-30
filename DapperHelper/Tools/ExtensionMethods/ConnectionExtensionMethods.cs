using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DapperHelper.Attributes;

namespace DapperHelper.Tools.ExtensionMethods
{
    /// <summary>
    /// Class to Manage the Access to the Database as well and act as an abstraction layer for 
    /// the CRUD operations
    /// </summary>
    public static class ConnectionExtensionMethods
    {
        /// <summary>
        /// Gets the connections string from the App.config file
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// A <see cref="string"/> containing the stored connection string from App.config
        /// </returns>
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        /// <summary>
        /// Execute an stored procedure <paramref name="storedProcedure"/> with input parameters <paramref name="parameters"/>
        /// from a database with the provided connection string <paramref name="connectionStringName"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionStringName"></param>
        /// <returns>
        /// A <see cref="List{T}"/> of <typeparamref name="T"/> containg the mapped Model 
        /// </returns>
        public static List<T> ReadData<T, U>(this IDbConnection dbConnection, string storedProcedure, U parameters, string connectionStringName)
        {
            return dbConnection.Query<T>(storedProcedure, parameters,
                 commandType: CommandType.StoredProcedure).ToList();

        }

        public static int Create<T>(this IDbConnection dbConnection, T model)
        {
            List<string> listOfCommands = new List<string>
            {
                QueryHelpers.cmdInsertInto,
                model.GetStringForQuery(new List<Type>(){typeof(UpdatableParAttribute)},  includeDataEncloser:false ,startHeader: model.GetTableName()),
                model.GetStringForQuery(new List<Type>(){typeof(UpdatableParAttribute)}, isValueString: true, startHeader: QueryHelpers.cmdValues)
            };
            return dbConnection.Execute(listOfCommands.FormatQuery());
        }

        public static int Create<T>(this IDbConnection dbConnection, List<T> models)
        {
            string typeName = "tempType";

            dbConnection.DropTypeIfExists(typeName);

            dbConnection.CreateTableTypeFromModel(typeName, models.First());
           
            List<string> listOfCommands = new List<string>
            {
                QueryHelpers.cmdInsertInto,
                models.First().GetStringForQuery(new List<Type>(){typeof(UpdatableParAttribute)}, includeDataEncloser:false, startHeader: models.First().GetTableName()),
                models.First().GetStringForQuery(new List<Type>(){typeof(UpdatableParAttribute)}, includeGroupEnclocer: false, startHeader: $"{QueryHelpers.cmdSelect}{QueryHelpers.space}",startDataEncloser:"[", endDataEncloser:"]"),
                QueryHelpers.cmdFrom,
                "@CustomTable",
            };
            return dbConnection.Execute(listOfCommands.FormatQuery(), param : new { CustomTable = models.GetDataTable().AsTableValuedParameter(typeName)});
        }

        public static int CreateTableTypeFromModel(this IDbConnection dbConnection, string name, object model)
        {

            List<string> listOfCommands = new List<string>
            {
                QueryHelpers.cmdCreateType,
                name,
                QueryHelpers.cmdAsTable,
                model.GetTypeDefinition()
                
            };
            return dbConnection.Execute(listOfCommands.FormatQuery());
        }


        public static int DropTypeIfExists(this IDbConnection dbConnection, string name)
        {
            List<string> listOfCommands = new List<string>()
            {
                QueryHelpers.cmdDropTypeIfExists,
                name
            };

            return dbConnection.Execute(listOfCommands.FormatQuery());
        }

        public static void Delete<T>(this IDbConnection dbConnection, int id)
        {
            throw new System.NotImplementedException();
        }

        public static List<T> Read<T>(this IDbConnection dbConnection, int id)
        {
            throw new System.NotImplementedException();
        }

        public static List<T> ReadAll<T>(this IDbConnection dbConnection)
        {
            throw new System.NotImplementedException();
        }

        public static void Update<T>(this IDbConnection dbConnection, T updatedModel)
        {
            throw new System.NotImplementedException();
        }

        public static void Update<T>(this IDbConnection dbConnection, List<T> updateModels)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Update a table through the execution of a stored procedure <paramref name="storedProcedure"/> with
        /// <paramref name="parameters"/> using the configured <paramref name="connectionStringName"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionStringName"></param>
        /// <returns>
        /// An <see cref="System.Int16"/> containig the number of affected rows
        /// </returns>
        public static int ExecuteStoreProcedure<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Executes an <see cref="SqlTransaction"/> depending on the selected <paramref name="command"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="command"></param>
        /// <returns>
        /// A <see cref="System.Int32"/> containing the numer of affected rows
        /// </returns>
        public static int ExecuteTransaction<T>(List<T> data, string connectionStringName, SqlActions command)
        {
            if (data?.Count > 0)
            {
                string connectionString = GetConnectionString(connectionStringName);

                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (IDbTransaction transaction = connection.BeginTransaction())
                    {
                        return 0;
                        // TODO: here modify
                        //return connection.Execute(data.FirstOrDefault().GetQueryStringForSqlTrans(command), data, transaction: transaction); 
                    }
                }
            }

            return 0;
        }
    }
}








