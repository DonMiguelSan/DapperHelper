using Dapper;
using DapperHelper.Interfaces;
using DapperHelper.Tools;
using DapperHelper.Tools.ExtensionMethods;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperHelper.Internal.DataAccess
{
    /// <summary>
    /// Class to Manage the Access to the Database as well and act as an abstraction layer for 
    /// the CRUD operations
    /// </summary>
    internal class SqlDataAccess
    {
        /// <summary>
        /// Gets the connections string from the App.config file
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// A <see cref="string"/> containing the stored connection string from App.config
        /// </returns>
        public string GetConnectionString(string name)
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
        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        /// <summary>
        /// Update a table through the execution of a stored procedure <paramref name="storedProcedure"/> with
        /// <paramref name="parameters"/> using the configured <paramref name="connectionStringName"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionStringName"></param>
        /// <returns>
        /// An <see cref="System.Int16"/> containig the number of affected rows
        /// </returns>
        public int ExecuteStoreProcedure<T,U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Execute(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Save a Model <typeparamref name="T"/> to a table using the stored procedure <paramref name="storedProcedure"/>
        /// with the provided connection string <paramref name="connectionStringName"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters"></param>
        /// <param name="connectionStringName"></param>
        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                _ = connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Executes an <see cref="SqlTransaction"/> depending on the selected <paramref name="command"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="command"></param>
        public void ExecuteTransaction<T>(List<T> data, string connectionStringName, SqlCommands command)
            where T: IDbTableModel
        {
            if (data?.Count > 0) 
            {
                string connectionString = GetConnectionString(connectionStringName);

                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    IDbTransaction transaction = connection.BeginTransaction();

                    _ = connection.Execute(data.FirstOrDefault().GetQueryStringForSqlTrans(command), data, transaction: transaction);
                } 
            }
        }
    }
}
