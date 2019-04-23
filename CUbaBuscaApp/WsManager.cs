using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUbaBuscaApp
{
    public static class WsManager
    {

        private static afipService.Service service = new afipService.Service();

        public static IEnumerable<dynamic> MonedasHomolog()
        {           
            afipService.Moneda[] monedas = service.FEParamGetTiposMonedas(getTicket()).ResultGet;
            DataContainer.Instance().dbManager.GenericHomolog(monedas, typeof(afipService.Moneda));
            return monedas;
        }

        public static IEnumerable<dynamic> IvassHomolog()
        {
            afipService.IvaTipo[] ivas = service.FEParamGetTiposIva(getTicket()).ResultGet;
            DataContainer.Instance().dbManager.GenericHomolog(ivas, typeof(afipService.IvaTipo));
            return ivas;
        }

        public static IEnumerable<dynamic> ConceptosHomolog()
        {            
            afipService.ConceptoTipo[] conceptos = service.FEParamGetTiposConcepto(getTicket()).ResultGet;
            DataContainer.Instance().dbManager.GenericHomolog(conceptos, typeof(afipService.ConceptoTipo));
            return conceptos;
        }

        public static IEnumerable<dynamic> TiposFacturaHomolog()
        {
            afipService.CbteTipo[] tipos = service.FEParamGetTiposCbte(getTicket()).ResultGet;
            DataContainer.Instance().dbManager.GenericHomolog(tipos, typeof(afipService.CbteTipo));
            return tipos;
        }

        public static IEnumerable<dynamic> PtosVentaHomolog()
        {
            afipService.PtoVenta[] puntos = service.FEParamGetPtosVenta(getTicket()).ResultGet;
            if (puntos != null)
                DataContainer.Instance().dbManager.GenericHomolog(puntos, typeof(afipService.PtoVenta));
            return puntos;
        }

        public static IEnumerable<dynamic> TiposDocHomolog()
        {
            afipService.DocTipo[] puntos = service.FEParamGetTiposDoc(getTicket()).ResultGet;
            if (puntos != null)
                DataContainer.Instance().dbManager.GenericHomolog(puntos, typeof(afipService.DocTipo));
            return puntos;
        }



        public static Factura Facturar(Factura factura,IEnumerable<FacturaDetalles> detalles,
            afipService.CbteTipo tipofacT,int pPtoVta,bool isAulacion,long nroanulado=0)
        {
            try
            {
                afipService.FECAECabRequest feCabReq = new afipService.FECAECabRequest()
                {
                    CantReg = detalles.Count(),
                    CbteTipo = tipofacT.Id,
                    PtoVta = pPtoVta
                };

                List<afipService.FECAEDetRequest> detRequests = new List<afipService.FECAEDetRequest>();
                int numeroComprobAutorizar = numeroComprobaAturizar(tipofacT.Id, pPtoVta);

                

                Clientes cliente = null;
                if (factura.clientdId != null)
                    cliente = DataContainer.Instance().dbManager.GetCliente((long)factura.clientdId);
                //Ivas
                double totalIva = 0;
                var ivas = new List<afipService.AlicIva>();
                foreach (var detalle in detalles)
                {
                    ivas.Add(new afipService.AlicIva()
                    {
                        BaseImp = (double)detalle.impuestobase,
                        Id = (int)detalle.impuestoId,
                        Importe = (double)detalle.impuestovalor
                    });
                    totalIva += (double)detalle.impuestovalor;

                }


                afipService.FECAEDetRequest detRequest = new afipService.FECAEDetRequest()
                {
                    CbteDesde = numeroComprobAutorizar,
                    CbteHasta = numeroComprobAutorizar,
                    CbteFch = factura.fechacreacion.ToString("yyyyMMdd"),                    
                    Concepto = (int)factura.conceptoId,                  
                    FchServDesde = DateTime.Now.ToString("yyyyMMdd"),
                    FchServHasta = DateTime.Now.ToString("yyyyMMdd"),
                    FchVtoPago = DateTime.Now.ToString("yyyyMMdd"),
                    DocNro=0,
                    DocTipo=99,
                    ImpIVA = totalIva,
                  
                    ImpOpEx = tipofacT.Id == 11 || tipofacT.Id == 13 ? 0: factura.total - totalIva,
                    ImpTotConc = tipofacT.Id == 11 || tipofacT.Id == 13 ? 0 : factura.total - totalIva,
                    ImpNeto = (double)factura.noGravado,
                    ImpTotal = factura.total,              
                    MonCotiz = 1,//Cambiar cuando sea facturacion en dolares bla bla bla
                    MonId = factura.monedaId,
                    Compradores = null,//*
                    Opcionales = null,//*
                    Tributos = null,//*
                    ImpTrib = 0,//*
                };
                if (isAulacion) {
                    List<afipService.CbteAsoc> asociados = new List<afipService.CbteAsoc>();

                    asociados.Add(new afipService.CbteAsoc()
                    {
                        CbteFch = factura.fechacreacion.ToString("yyyyMMdd"),
                        Cuit = getTicket().Cuit.ToString(),
                        Nro = nroanulado,
                        PtoVta = pPtoVta,
                        Tipo = tipofacT.Id

                    });
                    detRequest.CbtesAsoc = asociados.ToArray();
                }
                //If factuar C
                if (tipofacT.Id != 11 && tipofacT.Id != 13) {                    
                    detRequest.Iva = ivas.ToArray();
                }
                if (cliente != null) {
                    detRequest.DocNro = long.Parse(factura.clientdId == null ? null : cliente.nrodoc);
                    detRequest.DocTipo = int.Parse(factura.clientdId == null ? null : cliente.idtipoDoc);
                }

                detRequests.Add(detRequest);


                afipService.FECAERequest request = new afipService.FECAERequest()
                {
                    FeCabReq = feCabReq,
                    FeDetReq = detRequests.ToArray(),

                };

                afipService.FECAEResponse response = service.FECAESolicitar(getTicket(), request);
                if (response.Errors==null)
                {
                    if (response.FeCabResp.Resultado == "A" || response.FeCabResp.Resultado == "P")
                    {
                   
                        factura.numeroFact = numeroComprobAutorizar;
                        factura.estadoId = 2;
                        factura.cae = response.FeDetResp[0].CAE;
                    }
                    else {
                        string observaciones = "";
                        foreach (var det in response.FeDetResp)
                            foreach (var o in det.Observaciones)
                                observaciones += o.Msg + Environment.NewLine;
                        factura.estadoEnvioAfiperror += observaciones;
                        factura.estadoId = 3;
                    }
                    
                }
                else {

                    factura.estadoEnvioAfiperror = string.Join("--", response.Errors.Select(a => a.Msg));
                    factura.estadoId = 3;
                }
                DataContainer.Instance().dbManager.WriteFactura(factura, detalles);
                return factura;
            }

            catch (Exception ex)
            {
                Logger.WriteLog("Factuarcion fallida");
                Logger.WriteLog(ex.Message);
                factura.estadoEnvioAfiperror = ex.Message;
                factura.estadoId = 6;               
                DataContainer.Instance().dbManager.WriteFactura(factura, detalles);
                return factura;
            }
        }

        private static int numeroComprobaAturizar(int cbteTipo,int ptoVenta)
        {

            var comprob = service.FECompUltimoAutorizado(getTicket(), ptoVenta, cbteTipo);
            if (comprob.Errors?.Count() > 0)
                throw new Exception("No se pudo consultar el ultimo comprobante autorizado " + 
                    String.Join(",", comprob.Errors.Select(e => e.Msg)));
            return comprob.CbteNro + 1;

            

        }


        private static afipService.FEAuthRequest getTicket() {
            var ticket = LoginManager.ObtenerTicket();
            var cuit = long.Parse(DataContainer.Instance().dbManager.ConfigByKey("cuit"));
           return new afipService.FEAuthRequest()
            {
                Sign = ticket.sign,
                Token = ticket.token,
                Cuit = cuit
            };
        }
    }
}
