using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afip
{
    public class WsManager
    {
        private Afip.AFIP_WS.ServiceSoapClient _service = new AFIP_WS.ServiceSoapClient();

        public WsManager() {

            
        }


        public void Login() {

            AFIP_WS.FEAuthRequest authRequest = new AFIP_WS.FEAuthRequest();
           // authRequest.
          //   _service.FECAEAConsultar()
        }
    }
}
