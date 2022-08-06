using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Messence.Shared
{
    public class ConfigurationHelper
    {
        #region Singleton
        private static readonly ConfigurationHelper instance = new ConfigurationHelper();
        // Explicit static constructor to tell C# compiler  
        // not to mark type as beforefieldinit  
        static ConfigurationHelper()
        {
        }
        private ConfigurationHelper()
        {
        }
        public static ConfigurationHelper Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion

        public string MessenceDBConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString; }
        }
    }
}
