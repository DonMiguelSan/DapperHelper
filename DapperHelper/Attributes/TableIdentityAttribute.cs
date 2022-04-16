using System;

namespace DapperHelper.Attributes
{
    /// <summary>
    /// <see cref="Attribute"/> to idenfity a model identity key
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class TableIdentityAttribute : Attribute
    {
    }
}
