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
    public partial class ServiociosForm : RadForm
    {
        public ServiociosForm()
        {
            InitializeComponent();
        }

        private void ServiociosForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
            radGridView1.DataSource = DataContainer.Instance().dbManager.ServiciosList();
            Helper.InicializarGrid(radGridView1, new[] { "Id" });          
            radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
            radGridView1.UserAddedRow += RadGridView1_UserAddedRow;
            radGridView1.UserDeletingRow += RadGridView1_UserDeletingRow;
        }

        private void RadGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var rows = radGridView1.SelectedRows;
            string mensaje = String.Empty;
            foreach (GridViewDataRowInfo row in rows) {
                var servicio = (Servicio)row.DataBoundItem;
                mensaje += DataContainer.Instance().dbManager.ManageServicio(servicio, true) + $" servocio:{servicio.Id}" + Environment.NewLine;
            }
            MessageManager.SowMessage(mensaje,ThemeName);
        }

        private void RadGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            MessageManager.SowMessage(DataContainer.Instance().dbManager.ManageServicio((Servicio)e.Row.DataBoundItem), ThemeName);
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem != null)
                MessageManager.SowMessage(DataContainer.Instance().dbManager.ManageServicio((Servicio)e.Row.DataBoundItem), ThemeName);
        }
    }
}
