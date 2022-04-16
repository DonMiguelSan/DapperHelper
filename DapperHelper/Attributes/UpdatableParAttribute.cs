using System;

namespace DapperHelper.Attributes
{
    /// <summary>
    /// <see cref="Attribute"/> to identify an updatable parameter in a stored procedure
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class UpdatableParAttribute : Attribute
    { 
    }
}
