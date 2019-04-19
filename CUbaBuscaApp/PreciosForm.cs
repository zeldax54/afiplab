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
        public PreciosForm()
        {
            InitializeComponent();
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
            radGridView2.UserAddedRow += AddPrecio;
            radGridView2.UserDeletingRow += DeletePrecio;

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
            if (e.Row.DataBoundItem != null)
                MessageManager.SowMessage(DataContainer.Instance().dbManager.EditarPrecio((Precio)e.Row.DataBoundItem), ThemeName);
        }

        private void RadGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var s = radGridView1.SelectedRows[0].DataBoundItem as Servicio;
            radGridView2.DataSource = DataContainer.Instance().dbManager.PreciosListByService(s.Id);
            Helper.InicializarGrid(radGridView2, new[] { "Id", "ServicioId" });

        }

        private void PreciosForm_SizeChanged(object sender, EventArgs e)
        {
            radGridView1.Width = this.Width / 2;
            radGridView2.Width = this.Width / 2;
        }

       

        
    }
}
