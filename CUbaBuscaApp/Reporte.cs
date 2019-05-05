

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





        }
    }
}