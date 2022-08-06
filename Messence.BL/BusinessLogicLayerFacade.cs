using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messence.BL
{
    public class Business_Logic_Layer_Facade
    {
        #region Singleton

        /***************** -- SINGLETON -- START  ********************/
        private static readonly Business_Logic_Layer_Facade instance = new Business_Logic_Layer_Facade();
        // Explicit static constructor to tell C# compiler  
        // not to mark type as beforefieldinit  
        static Business_Logic_Layer_Facade()
        {
        }

        private Business_Logic_Layer_Facade()
        {
        }

        public static Business_Logic_Layer_Facade Instance
        {
            get
            {
                return instance;
            }
        }
        /***************** -- SINGLETON -- END  ********************/

        #endregion


    }
}
