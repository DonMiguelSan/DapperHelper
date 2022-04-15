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
        protected string spCreate;

        protected string spReadAll;

        protected string spUpdate;

        protected string spSearch;

        public virtual List<T> ReadAll()
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                return sql.LoadData<T, dynamic>(spReadAll, null, SqlConstants.dataBase);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public virtual void Create(T model)
        {
            SqlDataAccess sqlDataAccess = new SqlDataAccess();

            sqlDataAccess.SaveData(spCreate, model, SqlConstants.dataBase);
        }

        public virtual List<T> Search(int id)
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                var p = new { id };

                var output = sql.LoadData<T, dynamic>(spSearch, p, SqlConstants.dataBase);

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual List<T> Search(object storedProcedurePar)
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                var output = sql.LoadData<T, dynamic>(spSearch, storedProcedurePar, SqlConstants.dataBase);

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public virtual void Update(int id, T updatedModel)
        {
            try
            {
                SqlDataAccess sql = new SqlDataAccess();

                var parameters = updatedModel.GetParameterObject(id, new List<Type> { typeof(UpdatableParAttribute), typeof(TableIdentityAttribute) });

                _ = sql.ExecuteStoreProcedure<T, dynamic>(spUpdate, parameters, SqlConstants.dataBase);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}