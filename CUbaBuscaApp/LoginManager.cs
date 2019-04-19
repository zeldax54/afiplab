using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUbaBuscaApp
{
    public static class LoginManager
    {


        public static Logininfo ObtenerTicket(int intentos=5) {
            try
            {
                if (intentos == 0)
                    throw new Exception("No se puede obtener el ticket");
            Logininfo lI = DataContainer.Instance().dbManager.GetLoginInfo();
            if (lI == null)
               return TicketServiceManager.generarLog();
            return lI;                                
            }

            catch (Exception e) {

                Logger.WriteLog("ObtenerTicket"+ e.Message + " intentos " + intentos);
                return ObtenerTicket(intentos - 1);
            }
            
        }
    }
}
