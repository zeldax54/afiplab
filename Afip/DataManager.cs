using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Afip
{
    public class DataManager
    {

       
        public DataManager() {             
            var dbManager = new SQLiteDb( ConfigurationManager.AppSettings["sqliteUrl"]);
        }
    }
}
