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
    public partial class PreciosForm : RadForm
    {
        bool Isfromfact = false;
        public Precio P=null;
        public DateTime FechaVigencia;
        public PreciosForm(DateTime fechaVigencia, bool pisfrofact=false)
        {
            InitializeComponent();           
            FechaVigencia = fechaVigencia;
            Isfromfact = pisfrofact;
            if (Isfromfact)
                radButton1.Visible = true;
        }

        private void PreciosForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(Controls, this);
            radGridView1.DataSource = DataContainer.Instance().dbManager.ServiciosList();         
            Helper.InicializarGrid(radGridView1, new[] { "Id" });
       
         
            radGridView1.MultiSelect = false;
            radGridView1.AllowEditRow = false;
            radGridView1.AllowAddNewRow = false;
            radGridView1.SelectionChanged += RadGridView1_SelectionChanged;
            this.SizeChanged += PreciosForm_SizeChanged;

            //Edit precios event
            radGridView2.CellEndEdit += EditarPrecio;
            radGridView2.CellBeginEdit += RadGridView2_CellBeginEdit;
            radGridView2.UserAddedRow += AddPrecio;
            radGridView2.UserDeletingRow += DeletePrecio;
            radGridView1.ClearSelection();
            if (Isfromfact)
            {
                radGridView1.AllowAddNewRow = false;
                radGridView2.AllowAddNewRow = false;

                Helper.InicializarGridReadObly(radGridView1, null);
                Helper.InicializarGridReadObly(radGridView2, null);

            }
            
        }

        private void RadGridView2_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            if (e.Column.Name.ToLower().Contains("iva"))
            {
                var ivaform = new IvaEditorForm();
                ivaform.ShowDialog();
                var imp = ivaform.IvaValue;
                var id = ivaform.Ivaid;
                e.Row.Cells[e.ColumnIndex].Value = imp;
                e.Row.Cells[e.ColumnIndex-1].Value = id;
            }
        }

        private void DeletePrecio(object sender, GridViewRowCancelEventArgs e)
        {
            var rows = radGridView2.SelectedRows;
            string mensaje = String.Empty;
            foreach (GridViewDataRowInfo row in rows)
            {
                var precio = (Precio)row.DataBoundItem;
                mensaje += DataContainer.Instance().dbManager.DeletePrecio(precio) + $" precio:{precio.Id}" + Environment.NewLine;
            }
            MessageManager.SowMessage(mensaje, ThemeName);
        }

        private void AddPrecio(object sender, GridViewRowEventArgs e)
        {
            var precio = (Precio)e.Row.DataBoundItem;
            var servicio = radGridView1.SelectedRows[0].DataBoundItem as Servicio;
            precio.ServicioId = servicio.Id;
            MessageManager.SowMessage(DataContainer.Instance().dbManager.AddPrecio(precio), ThemeName);
        }

        private void EditarPrecio(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem != null) {
                var precio = (Precio)e.Row.DataBoundItem;
                var ret = DataContainer.Instance().dbManager.EditarPrecio(precio);
                if ((bool)ret[1] == false) {
                    e.Row.Cells["precio"].Value = ((Precio)ret[2]).precio;
                }
                MessageManager.SowMessage(ret [0].ToString(),ThemeName);
             
            }
              
        }

        private void RadGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (radGridView1.SelectedRows.Any()) {
              
                var s = radGridView1.SelectedRows[0].DataBoundItem as Servicio;
                radGridView2.DataSource = DataContainer.Instance().dbManager.PreciosListByService(s.Id,FechaVigencia,Isfromfact);
                Helper.InicializarGrid(radGridView2, new[] { "Id", "ServicioId", "ivaId" });
                if (Isfromfact)
                {
                    radGridView2.AllowAddNewRow = false;
                    Helper.InicializarGridReadObly(radGridView2, null);
                    radGridView2.CellDoubleClick += RadGridView2_CellDoubleClick;
                }

                
            }           

        }

        private void RadGridView2_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            P = (Precio)e.Row.DataBoundItem;
            this.Close();           
        }

       

        //private void RadGridView2_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        ContextMenu m = new ContextMenu();
        //        MenuItem menuItem = new MenuItem("Agregar precio");
        //        menuItem.Click += MenuItem_Click;
        //        m.MenuItems.Add(menuItem);
        //        m.Show(radGridView2, e.Location);
        //    }
        //}

      

        
        
        private void PreciosForm_SizeChanged(object sender, EventArgs e)
        {
            radGridView1.Width = (int)(this.Width * 0.3);
            radGridView2.Width = (int)(this.Width * 0.7);

            radGridView1.Height = (int)(this.Height * 0.7);
            radGridView2.Height = (int)(this.Height * 0.7);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!radGridView2.SelectedRows.Any())
                MessageManager.SowMessage("No hay precios para agregar a la factura.", ThemeName);
            else
            {
                P = (Precio) radGridView2.SelectedRows[0].DataBoundItem;
                this.Close();
            }
                
            
        }
    }
}
