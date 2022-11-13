using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperHelper.Tools.ExtensionMethods
{
    public static class PropertyInfoExtensionMethods
    {
        public static Dictionary<List<string>, object> GetCustomAttrCtorVals<T>(this PropertyInfo propertyInfo)
        {
            Dictionary<List<string>, object> output = new Dictionary<List<string>, object>();

            var columNameAttrData = propertyInfo.CustomAttributes?.Where(x => x.AttributeType == typeof(T));

            foreach (var attr in columNameAttrData)
            {
                foreach (var ctorArg in attr.ConstructorArguments)
                {
                    // TODO: Finish Implementation

                }
            }

            return output;
        }
    }
}
