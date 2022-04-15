using DapperHelper.Attributes;
using System;

namespace DapperHelper.Interfaces
{
    public interface IDbTableModel
    {
        int Id { get; set; }

        string Description { get; set; }

        DateTime CreatedOn { get; set; }

        DateTime ModifiedOn { get; set; }
    }
}