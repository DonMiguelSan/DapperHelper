using DapperHelper.Attributes;
using System;

namespace DapperHelper.Interfaces
{
    /// <summary>
    /// Interface defining the minimum requirements that a table model must have
    /// </summary>
    public interface IDbTableModel
    {
        /// <summary>
        /// Id Primary key
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Created on Date and time -> Manage by the Database
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Modified on Date and time
        /// </summary>
        DateTime ModifiedOn { get; set; }
    }
}