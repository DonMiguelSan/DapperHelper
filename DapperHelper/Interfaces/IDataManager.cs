using System.Collections.Generic;

namespace DapperHelper.Interfaces
{
    public interface IDataManager<T> where T : IDbTableModel
    {
        List<T> ReadAll(string storedProcedure);
        void Create(T model, string storedProcedure);
        List<T> Search(int id, string storedProcedure);
        void Update(int id, T updatedModel, string storedProcedure);
    }
}