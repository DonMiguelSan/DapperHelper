using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHelper.Tools
{
    public static class ConfigManagerHelper
    {
        /// <summary>
        /// Gets the connections string from the App.config file
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        /// A <see cref="string"/> containing the stored connection string from App.config
        /// </returns>
        public static string GetConnectionString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString + ";" +
                "Integrated Security = SSPI;" + "MultipleActiveResultSets = True;";
        }

    }
}
