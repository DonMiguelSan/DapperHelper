using System.Collections.Generic;

namespace DapperHelper.Interfaces
{
    /// <summary>
    /// Interface to manage crud in table models
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataManager<T> where T : IDbTableModel
    {
        /// <summary>
        /// Executes a stored procedure <paramref name="storedProcedure"/> to read all 
        /// members of the table
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns>
        /// A <see cref="List{T}"/> where <see cref="T"/> is the model to be mapped
        /// </returns>
        List<T> ReadAll(string storedProcedure);

        /// <summary>
        /// Executes a stored procedure <paramref name="storedProcedure"/> to read all 
        /// members of the table
        /// </summary>
        /// <param name="storedProcedure"></param>
        /// <returns>
        /// A <see cref="List{T}"/> where <see cref="T"/> is the model to be mapped
        /// </returns>
        void Create(T model, string storedProcedure);

        /// <summary>
        /// Executes an stored procedure to search by <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="storedProcedure"></param>
        /// <returns>
        /// A <see cref="List{T}"/> where <see cref="T"/> is the datamodel to be mapped
        /// </returns>
        List<T> Search(int id, string storedProcedure);

        /// <summary>
        /// Executes a stored procedure to update a model <see cref="T"/> with a defined <paramref name="id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <param name="storedProcedure"></param>
        void Update(int id, T updatedModel, string storedProcedure);
    }
}