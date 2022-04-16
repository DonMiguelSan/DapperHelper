using DapperHelper.Attributes;
using DapperHelper.Interfaces;
using DapperHelper.Internal.DataAccess;
using DapperHelper.Tools;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace DapperHelper.DataAccess.Abstract
{
    /// <summary>
    /// Abstract class to implemet CRUD methods in every datamanger 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataManagerBase<T> : IDataManager<T> where T : IDbTableModel
    {
        /// <summary>
        /// Executes an stored procedure to retrieve all data 
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns>
        /// A <see cref="List{T} "/> where <see cref="T"/> is the required model to be mapped by dapper
        /// </returns>
        /// <exception cref="Exception"></exception>
        public virtual List<T> ReadAll(string storedProcedure)
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                return sql.LoadData<T, dynamic>(storedProcedure, null, SqlConstants.dataBase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Executes the stored procedure <paramref name="storedProcedure"/> to store the <paramref name="model"/>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="storedProcedure"></param>
        public virtual void Create(T model, string storedProcedure)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            sqlDataAccess.SaveData(storedProcedure, model, SqlConstants.dataBase);
        }

        /// <summary>
        /// Executes the store procedure to search by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storedProcedure"></param>
        /// <returns>
        /// A <see cref="List{T}"/> where <see cref="T"/> is the model to be mapped by dapper
        /// </returns>
        /// <exception cref="Exception"></exception>
        public virtual List<T> Search(int id, string storedProcedure)
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                var p = new { id };

                var output = sql.LoadData<T, dynamic>(storedProcedure, p, SqlConstants.dataBase);

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Excutes an stored procedure to update a <paramref name="updatedModel"/> with a selected <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <param name="storedProcedure"></param>
        /// <exception cref="Exception"></exception>
        public virtual void Update(int id, T updatedModel, string storedProcedure)
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                var parameters = updatedModel.GetParameterObject(id, new List<Type> { typeof(UpdatableParAttribute), typeof(TableIdentityAttribute) });

                _ = sql.ExecuteStoreProcedure<T, dynamic>(storedProcedure, parameters, SqlConstants.dataBase);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}