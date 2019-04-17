using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUbaBuscaApp
{
   public static class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Logger));
        public static void WriteLog(string mensaje)
        {

            Log.Info(mensaje);
        }
    }
}
