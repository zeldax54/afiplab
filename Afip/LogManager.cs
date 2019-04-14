using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Afip
{
    public static class Logger
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));
        public static void WriteLog(string mensaje) {
          
           Log.Info(mensaje);
        }
    }
}
