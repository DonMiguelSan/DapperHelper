using System;

namespace DapperHelper.Attributes
{
    /// <summary>
    /// <see cref="Attribute"/> to identify an Identifier from an external table
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public sealed class ExternalKeyIdentityAttribute : Attribute
    {

    }
}
