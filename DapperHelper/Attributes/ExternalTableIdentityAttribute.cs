using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper.Attributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    sealed class ExternalTableIdentityAttribute : Attribute
    {
       
    }
}
