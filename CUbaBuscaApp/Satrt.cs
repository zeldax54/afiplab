﻿using System;
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

        private void Satrt_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(); 

            Logger.WriteLog("Aplicacion iniciada");
            BdManager bd = new BdManager();
            DataContainer.Instance().dbManager = bd;
            string tema = DataContainer.Instance().dbManager.ConfigByKey("tema");
            DataContainer.Instance().Themename = tema;
            Helper.SetTheme(this.Controls,this);

            radMenuComboItem2.Items.Add("Dark");
            radMenuComboItem2.Items.Add("Light");
            radMenuComboItem2.Items.Add("Metro");
            radMenuComboItem2.Items.Add("Blue");
            radMenuComboItem2.Items.Add("Inicial");

            radMenuComboItem2.Items.Where(a=>a.Text.ToLower().Contains(tema)).First().Selected = true;
            radMenuComboItem2.ComboBoxElement.SelectedIndexChanged += ComboBoxElement_SelectedIndexChanged;

            radLabel1.Text = "Facturas de hoy " + DateTime.Now.Date.ToString("dd-MM-yyyy");
            FormatoGrid();
            
            this.SizeChanged += Satrt_SizeChanged;
            radGridView1.CellFormatting += RadGridView1_CellFormatting;

            radGridView1.CellDoubleClick += RadGridView1_CellDoubleClick;
            WindowState = FormWindowState.Maximized;

        }

        private void RadGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {

            Factura f = (Factura)e.Row.DataBoundItem;
            if (f.estadoId == 3 || f.estadoId == 6 || f.estadoId==1) {
                MessageManager.SowMessage("Comprobante rechazado o no emitido, se puede desechar.", ThemeName);
                return;
            }
                
            IEnumerable< FacturaDetalles> detalles = DataContainer.Instance().dbManager.DetallesFromFactura((int)f.Id);
            new FacturaForm(f, detalles).ShowDialog();
            FormatoGrid();
        }

        private void RadGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
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
            new PreciosForm().ShowDialog();
        }


        private void radMenuItem13_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
           var lista= WsManager.MonedasHomolog();
           Cursor = Cursors.Default;
           DialogResult d= MessageManager.SowMessage("Monedas descargadas. Desea visualizarlas?",ThemeName,false,true);
           if (d == DialogResult.Yes) 
                new Homologaciones(lista).ShowDialog();           

          
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
            FormatoGrid();
          
        }

        private void FormatoGrid() {
            radGridView1.DataSource = null;
            radGridView1.SummaryRowsBottom.Clear();
            radGridView1.SummaryRowsTop.Clear();
            radGridView1.DataSource = DataContainer.Instance().dbManager.FacturasbyFecha(DateTime.Now);
            Helper.InicializarGrid(radGridView1, new[] { "Id", "estadoId", "monedaId", "conceptoId", "clientdId", "cbteId", "originalidfact" });
            radGridView1.AllowAddNewRow = false;
            radGridView1.AllowEditRow = false;
            foreach (var row in radGridView1.Rows) 
                if (row.Cells["letrafact"].Value.ToString().Contains("Nota de Crédito") && row.Cells["estadoId"].Value.ToString()=="2")
                    row.Cells["total"].Value = Helper.myparseFloat( row.Cells["total"].Value.ToString())  * -1;
            

            GridViewSummaryItem summaryItem = new GridViewSummaryItem("total", "Facturado: {0}", GridAggregateFunction.Sum);
            
            GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();
            summaryRowItem.Add(summaryItem);
            this.radGridView1.SummaryRowsTop.Add(summaryRowItem);
            this.radGridView1.SummaryRowsBottom.Add(summaryRowItem);

        }
    }
}
