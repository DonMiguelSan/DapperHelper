using System;

namespace DapperHelper.Attributes
{
    /// <summary>
    /// <see cref="Attribute"/> to identify a stored procedure parameter in a model 
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class StoredProcedureParAttribute : Attribute
    {

    }
}
