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
    public partial class FacturacionRapida : RadForm
    {
        public FacturacionRapida()
        {
            InitializeComponent();
        }

        private void FacturacionRapida_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
            radGridView1.DataSource = DataContainer.Instance().dbManager.ListaFacR();
            Helper.InicializarGrid(radGridView1, new[] { "id","servicioId", "percioId", "ivaId" });
            //Helper.InicializarGridReadObly(radGridView1, new[] { "preciodesc", });
            radGridView1.CellClick += RadGridView1_CellClick;
            radGridView1.UserAddedRow += RadGridView1_UserAddedRow;
            radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
            this.SizeChanged += FacturacionRapida_SizeChanged;
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            CalcularMontos(e.Row);
            DataContainer.Instance().dbManager.EditCOnfRapida((facturacionRapidaconf)e.Row.DataBoundItem);
        }

        private void FacturacionRapida_SizeChanged(object sender, EventArgs e)
        {
            radGridView1.Width = (int)(this.Width * 0.9);
            radGridView1.Height = (int)(this.Height * 0.6);
        }

        private void RadGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            CalcularMontos(e.Row);           
            e.Row.Cells["id"].Value = DataContainer.Instance().dbManager.AddCOnfRapida((facturacionRapidaconf)e.Row.DataBoundItem);
        }

        private void RadGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name.ToLower().Contains("precio") && e.Value == null)
            {
                var preciosForm = new PreciosForm(DateTime.Now, true);
                preciosForm.ShowDialog();
                var precio = preciosForm.P;
                if (precio != null)
                {
                    e.Row.Cells["servicioId"].Value = precio.ServicioId;
                    e.Row.Cells["percioId"].Value = precio.Id;
                    e.Row.Cells["ivaId"].Value = precio.ivaId;
                    e.Row.Cells["porciva"].Value = precio.ivamonto;                    
                    e.Row.Cells["preciodesc"].Value = precio.descripcion;
                    e.Row.Cells["cantidad"].Value = 0;
                    e.Row.Cells["precio"].Value = precio.precio;

                }
            }
        }


        private void CalcularMontos(GridViewRowInfo row) {
            if (row.DataBoundItem != null)
            {
                var detalle = (facturacionRapidaconf)row.DataBoundItem;
                var total = detalle.precio * detalle.cantidad;
                var ivamonto =row.Cells["total"].Value = total;
                float impuestos = 0;
                if (detalle.ivavalue > 0)
                {
                    impuestos = (float)(total / (1 + (detalle.ivavalue / 100)));
                    row.Cells["impuestovalor"].Value = total - Math.Round(impuestos);
                }               

            }

        }
    }
}
