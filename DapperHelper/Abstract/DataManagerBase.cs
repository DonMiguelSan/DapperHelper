using DapperHelper.Attributes;
using DapperHelper.DataAccess;
using DapperHelper.Interfaces;
using DapperHelper.Internal.DataAccess;
using DapperHelper.Tools.ExtensionMethods;
using System;
using System.Collections.Generic;

namespace  DapperHelper.DataAccess.Abstract
{
    public abstract class DataManagerBase<T> : IDataManager<T> where T : IDbTableModel
    {
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

        public virtual void Create(T model, string storedProcedure)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            sqlDataAccess.SaveData(storedProcedure, model, SqlConstants.dataBase);
        }

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