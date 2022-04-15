using System.Collections.Generic;

namespace DapperHelper.Interfaces
{
    public interface IDataManager<T> where T : IDbTableModel
    {
        List<T> ReadAll();
        void Create(T model);
        List<T> Search(int id);
        List<T> Search(dynamic storedProcedurePar);
        void Update(int id, T updatedModel);
    }
}