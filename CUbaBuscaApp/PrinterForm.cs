using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Reporting;
using Telerik.WinControls.UI;

namespace CUbaBuscaApp
{
    public partial class PrinterForm : RadForm
    {
        private Factura _factura;
        private IEnumerable<FacturaDetalles> _detalles;
        public PrinterForm(Factura f = null, IEnumerable<FacturaDetalles> detalles=null)
        {
            InitializeComponent();
            if (f != null)
                _factura = f;
            if (detalles != null)
                _detalles = detalles;

            //test
            if (f == null)
            {
                _factura = DataContainer.Instance().dbManager.FacturaFromId(34);
                _detalles = DataContainer.Instance().dbManager.DetallesFromFactura(34);
            }
        }

        private void PrinterForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls,this);
            Reporte r =new Reporte(_factura,_detalles);
            reportViewer1.Margin=new Padding(0,0,0,0);
            reportViewer1.ReportSource = r;
            reportViewer1.RefreshReport();
            WindowState = FormWindowState.Maximized;
            //reportViewer1.ReportSource = reportBook;
        }
    }
}
