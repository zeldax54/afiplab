using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CUbaBuscaApp
{
    class DataContainer
    {
        private static DataContainer oInstance;


        public BdManager dbManager;





        protected DataContainer()
        {

        }




        public static DataContainer Instance()
        {
            if (oInstance == null)
                oInstance = new DataContainer();
            return oInstance;
        }

    }
}
