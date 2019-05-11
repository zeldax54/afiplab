

using System.Globalization;
using System.Linq;

namespace CUbaBuscaApp
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Collections.Generic;
    using CUbaBuscaApp.Properties;
    /// <summary>
    /// Summary description for Reporte.
    /// </summary>
    public partial class Reporte : Telerik.Reporting.Report
    {
       

        public Reporte(){
           
            InitializeComponent();
        }
        public Reporte(Factura factura, IEnumerable<FacturaDetalles> detalles)
        {

            var facturaWS = WsManager.GetFactura(factura);
         
            InitializeComponent();
            logopic.Value = "Resources/logo.png";
            empresatextBox.Value = DataContainer.Instance().dbManager.ConfigByKey("empresa");
            razonsocialvalue.Value= DataContainer.Instance().dbManager.ConfigByKey("razonsocial");
            domiciliovalue.Value= DataContainer.Instance().dbManager.ConfigByKey("domicilioempresa");
            condicionivavalue.Value= DataContainer.Instance().dbManager.ConfigByKey("condicionfreteiva");
            facturaletraText.Value = factura.letrafact.Split(new []{' '})[factura.letrafact.Split(new[] { ' ' }).Length-1];
            codigocomp.Value = Helper.StrZero(factura.cbteId.ToString(),2);
            comprobnombre.Value = Helper.ExepLastLetter(factura.letrafact);
            ptovetavalue.Value ="Punto de venta: "+ Helper.ToStrZero(factura.ptovta.ToString());
            compnrovalue.Value = "Comp. Nro: " + Helper.ToStrZero(factura.numeroFact.ToString());
            cuitvalue.Value="CUIT: " + DataContainer.Instance().dbManager.ConfigByKey("cuit");
            ibvalue.Value = "Ingresos Brutos: " + DataContainer.Instance().dbManager.ConfigByKey("cuit");
            fecinivalue.Value = "Fec. Inicio Actividades: " + DataContainer.Instance().dbManager.ConfigByKey("fecinicioactivi");

            periododesde.Value = "Perido facturado desde: " + FormatFecha(facturaWS.FchServDesde);
            peridohasta.Value = "Perido facturado hasta: " + FormatFecha(facturaWS.FchServHasta);
            fechavencimientopago.Value = "Fecha de vencimiento para el pago: " + FormatFecha(facturaWS.FchVtoPago);

            if (factura.clientdId != null)
            {
                Clientes c = DataContainer.Instance().dbManager.GetCliente((long)factura.clientdId);
                clientenrodoc.Value += " " + c.nrodoc;
                clientenombre.Value += " " + c.Nombre;
                concicioniva.Value += " " + c.conceptiva;
                var formapago = DataContainer.Instance().dbManager.FormapagoById((long)factura.formapagoId);
                condicioventa.Value += " "+formapago.descripcion;
                domicilio.Value += " " + c.docmicilio;
            }



            int detc = 0;
            foreach (var detalle in detalles)
            {
                //idproducto
                
                NewTextBox(Helper.ToStrZero(detalle.precioId.ToString(), 4),
                    Helper.ToStrZero(detalle.precioId.ToString(), 4), detc, 0);
                //Prod/serv
                NewTextBox(detalle.serviciodesc,
                    detalle.serviciodesc + "/" + detalle.preciodesc, detc, 1);
                //cantidad
                NewTextBox(detalle.cantidad.ToString(),
                    detalle.cantidad.ToString(), detc, 2);
                //UM
                NewTextBox("U",
                    "U", detc, 3);
                //Precio
                NewTextBox(detalle.precio.ToString(),
                    detalle.precio.ToString(), detc, 4);
                //Iva
                
                NewTextBox(detalle.impuestovalor.ToString(),
                    detalle.impuestovalor.ToString(), detc, 5);
                //Total
                NewTextBox(detalle.total.ToString(),
                    detalle.total.ToString(), detc, 6);
                detc++;
            }

            caebarcode.Value = factura.cae;
            vencimientocae.Value = FormatFecha(facturaWS.FchVto);
            fechafactfooter.Value = factura.fechacreacion.ToString("dd/MM/yyy");
            textBox50.Value = "Total tributos: " + (Math.Abs(factura.total) - Math.Abs(factura.noGravado ?? 0));
            textBox58.Value = "Importe total: " + Math.Abs(factura.total);

        }

        private string FormatFecha(string fecha)
        {
            return DateTime.ParseExact(fecha, "yyyyMMdd", new DateTimeFormatInfo()).ToString("dd/MM/yyy");
        }

        public void NewTextBox(string name,string valor,int rowindex,int columindex)
        {
            Telerik.Reporting.TextBox tmpTextBox = new Telerik.Reporting.TextBox();
            tmpTextBox = new Telerik.Reporting.TextBox();
            tmpTextBox.Style.Font.Name = DataContainer.Instance().dbManager.ConfigByKey("fuente");
            tmpTextBox.Size = new SizeU(Unit.Cm(0.08), Unit.Cm(0.5));
            tmpTextBox.Style.TextAlign = HorizontalAlign.Center;
            tmpTextBox.Name = name;
            tmpTextBox.Value = valor;
            productostable.Body.SetCellContent(rowindex, columindex, tmpTextBox);
        }
    }
}