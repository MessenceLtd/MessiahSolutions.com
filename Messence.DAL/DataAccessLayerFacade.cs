using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messence.DAL
{
    public class Data_Access_Layer_Facade
    {
        #region Singleton
        private static readonly Data_Access_Layer_Facade instance = new Data_Access_Layer_Facade();
        // Explicit static constructor to tell C# compiler  
        // not to mark type as beforefieldinit  
        static Data_Access_Layer_Facade()
        {
        }
        private Data_Access_Layer_Facade()
        {
        }
        public static Data_Access_Layer_Facade Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion


    }
}
