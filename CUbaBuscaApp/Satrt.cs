using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace CUbaBuscaApp
{
    public partial class Satrt : RadForm
    {
        public Satrt()
        {
            InitializeComponent();
        }

        private DateTime _lastSearchedDate;
        private void Satrt_Load(object sender, EventArgs e)
        {
            string tema = DataContainer.Instance().Themename;
            Helper.SetTheme(this.Controls,this);
            radGridView1.AutoSizeRows = false;
           
            radGridView1.Columns.Add(new GridViewCommandColumn
            {
                HeaderText = "Imprimir",
                Name = "Imprimir",
                Width = 50,
                IsVisible = false
            });
           
            radMenuComboItem2.Items.Add("Dark");
            radMenuComboItem2.Items.Add("Light");
            radMenuComboItem2.Items.Add("Metro");
            radMenuComboItem2.Items.Add("Blue");
            radMenuComboItem2.Items.Add("Inicial");

            radMenuComboItem2.Items.First(a => a.Text.ToLower().Contains(tema)).Selected = true;
            radMenuComboItem2.ComboBoxElement.SelectedIndexChanged += ComboBoxElement_SelectedIndexChanged;

            radLabel1.Text = "Facturas de hoy " + DateTime.Now.Date.ToString("dd-MM-yyyy");
            FormatoGrid(DateTime.Now);
            
            this.SizeChanged += Satrt_SizeChanged;
            radGridView1.CellFormatting += RadGridView1_CellFormatting;

            radGridView1.CellDoubleClick += RadGridView1_CellDoubleClick;
            WindowState = FormWindowState.Maximized;
            radDateTimePicker1.Value = DateTime.Now;
         
        }

        private void RadGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (!e.Column.Name.Contains("Imprimir"))
            {
                Factura f = (Factura)e.Row.DataBoundItem;
                if (f != null)
                {
                    if (f.estadoId == 3 || f.estadoId == 6 || f.estadoId == 1)
                    {
                        MessageManager.SowMessage("Comprobante rechazado o no emitido, se puede desechar.", ThemeName);
                        return;
                    }
                }

                IEnumerable<FacturaDetalles> detalles = DataContainer.Instance().dbManager.DetallesFromFactura((int)f.Id);
                new FacturaForm(f, detalles).ShowDialog();
                FormatoGrid(radDateTimePicker1.Value);
            }
            else
            {
                Factura f = (Factura)e.Row.DataBoundItem;
                var detalles = DataContainer.Instance().dbManager.DetallesFromFactura((int)f.Id);
                new PrinterForm(f, detalles).ShowDialog();
            }
        }

        private void RadGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
          
            if (e.Column.Name == "Imprimir")
            {
                GridCommandCellElement cmdCell = e.CellElement as GridCommandCellElement;
                cmdCell.CommandButton.ImageAlignment = ContentAlignment.MiddleCenter;
                cmdCell.CommandButton.Image = Properties.Resources.Document;
            }
            if (e.Column.Name== "estadodesc" && e.CellElement.Value!=null)
            {
                e.CellElement.DrawFill = true;
                switch (e.CellElement.Value.ToString()) {

                    case "APROBADO":
                        e.CellElement.BackColor = Color.LawnGreen;
                        break;
                    case "RECHAZADO":
                        e.CellElement.BackColor = Color.OrangeRed;
                        break;
                    case "ANULADO":
                        e.CellElement.BackColor = Color.DeepSkyBlue;
                        break;
                    case "ERROR":
                        e.CellElement.BackColor = Color.Red;
                        break;
                }          
            }              
        }

        private void Satrt_SizeChanged(object sender, EventArgs e)
        {
            radGridView1.Width = (int)(this.Width * 0.9);
            radGridView1.Height = (int)(this.Height * 0.8);

        }

        private void ComboBoxElement_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (e.Position != -1) {
                DataContainer.Instance().Themename = radMenuComboItem2.Items[e.Position].Text;
                Helper.SetTheme(Controls, this);
            }        
        }

       
      

        private void radMenuItem3_Click(object sender, EventArgs e)
        {
            new Configuracion().ShowDialog();
        }

        private void radMenuItem7_Click(object sender, EventArgs e)
        {
            new ServiociosForm().ShowDialog();
        }

     

        private void radMenuItem12_Click(object sender, EventArgs e)
        {
            new Configuracion().ShowDialog();

        }

        private void radMenuItem14_Click(object sender, EventArgs e)
        {
            new ServiociosForm().ShowDialog();
        }

       

        private void radMenuItem9_Click_1(object sender, EventArgs e)
        {
            new PreciosForm(DateTime.Now).ShowDialog();
        }


        private void radMenuItem13_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
           var lista= WsManager.MonedasHomolog();
           Cursor = Cursors.Default;
           DialogResult d= MessageManager.SowMessage("Monedas descargadas. Desea visualizarlas?",ThemeName,false,true);
           if (d == DialogResult.Yes) 
                new Homologaciones(lista).ShowDialog();

            Cursor = Cursors.Default;
        }        
       

        private void radMenuItem15_Click(object sender, EventArgs e)
        {
            new Homologaciones(DataContainer.Instance().dbManager.GenericTable("moneda")).ShowDialog();
        }

        private void radMenuItem16_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var lista = WsManager.IvassHomolog();
            Cursor = Cursors.Default;
            DialogResult d = MessageManager.SowMessage("Ivas descargados. Desea visualizarlaos?", ThemeName, false, true);
            if (d == DialogResult.Yes)
                new Homologaciones(lista).ShowDialog();
            Cursor = Cursors.Default;
        }

        private void radMenuItem17_Click(object sender, EventArgs e)
        {
            new Homologaciones(DataContainer.Instance().dbManager.GenericTable("iva")).ShowDialog();
        }

        private void radMenuItem19_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var lista = WsManager.ConceptosHomolog();
            Cursor = Cursors.Default;
            DialogResult d = MessageManager.SowMessage("Conceptos descargados. Desea visualizarlaos?", ThemeName, false, true);
            if (d == DialogResult.Yes)
                new Homologaciones(lista).ShowDialog();
        }

        private void radMenuItem20_Click(object sender, EventArgs e)
        {
            new Homologaciones(DataContainer.Instance().dbManager.GenericTable("conceptos")).ShowDialog();
        }

        private void radMenuItem22_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var lista = WsManager.TiposFacturaHomolog();
            Cursor = Cursors.Default;
            DialogResult d = MessageManager.SowMessage("Cbte descargados. Desea visualizarlaos?", ThemeName, false, true);
            if (d == DialogResult.Yes)
                new Homologaciones(lista).ShowDialog();
        }

        private void radMenuItem23_Click(object sender, EventArgs e)
        {
            new Homologaciones(DataContainer.Instance().dbManager.GenericTable("cbte")).ShowDialog();
        }

        private void radMenuItem24_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void radMenuItem26_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var lista = WsManager.PtosVentaHomolog();
            Cursor = Cursors.Default;
            if (lista == null)
                MessageManager.SowMessage("No hay puntos de venta registrados.", ThemeName);
            else
            {
                DialogResult d = MessageManager.SowMessage("Puntos descargados. Desea visualizarlaos?", ThemeName, false, true);
                if (d == DialogResult.Yes)
                    new Homologaciones(lista).ShowDialog();
            }          
        }

        private void radMenuItem27_Click(object sender, EventArgs e)
        {
            new Homologaciones(DataContainer.Instance().dbManager.GenericTable("ptovta")).ShowDialog();
        }

        private void radMenuItem29_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var lista = WsManager.TiposDocHomolog();
            Cursor = Cursors.Default;
            DialogResult d = MessageManager.SowMessage("Tipos de documentos descargados. Desea visualizarlaos?", ThemeName, false, true);
            if (d == DialogResult.Yes)
                new Homologaciones(lista).ShowDialog();
        }

        private void radMenuItem30_Click(object sender, EventArgs e)
        {
            new Homologaciones(DataContainer.Instance().dbManager.GenericTable("docs")).ShowDialog();
        }

        private void radMenuItem5_Click(object sender, EventArgs e)
        {
            new FacturaForm().ShowDialog();
            FormatoGrid(radDateTimePicker1.Value);
          
        }

        private void FormatoGrid(DateTime t) {
            radGridView1.DataSource = null;
            radGridView1.SummaryRowsBottom.Clear();
            radGridView1.SummaryRowsTop.Clear();
            radGridView1.DataSource = DataContainer.Instance().dbManager.FacturasbyFecha(t);
            Helper.InicializarGrid(radGridView1, new[] { "Id", "estadoId", "monedaId", "conceptoId", "clientdId", "cbteId", "originalidfact" });
            radGridView1.AllowAddNewRow = false;
            radGridView1.AllowEditRow = false;
            foreach (var row in radGridView1.Rows) 
                if (row.Cells["letrafact"].Value.ToString().Contains("Nota de Crédito") && row.Cells["estadoId"].Value.ToString()=="2")
                    row.Cells["total"].Value = Helper.myparseFloat( row.Cells["total"].Value.ToString())  * -1;
            radGridView1.Columns[0].IsVisible = false;
            radGridView1.EnableFiltering = false;
            if (radGridView1.Rows.Count > 0)
            {
                if(radGridView1.Rows.Count>10)
                    radGridView1.EnableFiltering = true;
                radGridView1.Columns[0].IsVisible = true;
                radGridView1.Columns[0].Width = 40;
                for (var index = 0; index < radGridView1.Rows.Count; index++)
                    radGridView1.Rows[index].Height = 30;
                  
                

                CustomSummaryItem summaryItem = new CustomSummaryItem("total", "Facturado: {0}",
                    GridAggregateFunction.Sum);
                GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();
                summaryRowItem.Add(summaryItem);
                this.radGridView1.SummaryRowsTop.Add(summaryRowItem);
                this.radGridView1.SummaryRowsBottom.Add(summaryRowItem);
            }
            radLabel1.Text = "Facturas de " + t.ToShortDateString();
        }

        private void radMenuItem14_Click_1(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var lista = WsManager.MonedasHomolog();
            Cursor = Cursors.Default;
            DialogResult d = MessageManager.SowMessage("Tipos de documentos descargados. Desea visualizarlaos?", ThemeName, false, true);
            if (d == DialogResult.Yes)
                new Homologaciones(lista).ShowDialog();
        }

        private void radDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            FormatoGrid(((RadDateTimePicker)sender).Value);
        }

        private void radMenuItem32_Click(object sender, EventArgs e)
        {
            new FacturacionRapida().ShowDialog();
        }

        private void radMenuItem33_Click(object sender, EventArgs e)
        {
            new FacturacionRapidaForm().ShowDialog();
            FormatoGrid(radDateTimePicker1.Value);
        }

        //private void RadGridView1_GroupSummaryEvaluate(object sender, GroupSummaryEvaluationEventArgs e)
        //{
        //    MasterGridViewTemplate t =(MasterGridViewTemplate) e.Parent;
        //    string letra = t.CurrentRow.Cells["letrafact"].Value.ToString();
        //    if (t.CurrentRow.Cells["estadoId"].Value.ToString() != "2" && t.CurrentRow.Cells["estadoId"].Value.ToString() != "5")
        //        e.Value = 0;
        //}
    }

    public class CustomSummaryItem : GridViewSummaryItem
    {
        public CustomSummaryItem(string name, string formatString, GridAggregateFunction aggregate)
            : base(name, formatString, aggregate)
        { }
        public override object Evaluate(IHierarchicalRow row)
        {
            float lowFreightsCount = 0;
            foreach (GridViewRowInfo childRow in row.ChildRows)
            {
                if (childRow.Cells["estadoId"].Value.ToString() == "2" || childRow.Cells["estadoId"].Value.ToString()=="5")
                {
                    lowFreightsCount += Helper.myparseFloat(childRow.Cells["total"].Value.ToString());
                }
            }
            return lowFreightsCount;
        }
    }
}
