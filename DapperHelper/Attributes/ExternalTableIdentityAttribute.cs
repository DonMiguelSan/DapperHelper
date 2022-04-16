using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper.Attributes
{
    /// <summary>
    /// <see cref="Attribute"/> to identify an Identifier from an external table
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    sealed class ExternalTableIdentityAttribute : Attribute
    {
       
    }
}
