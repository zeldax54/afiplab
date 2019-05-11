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
            Helper.InicializarGrid(radGridView1, new[] { "id","servicioId", "percioId", "ivaId","cbteid","conceptoid" });
            //Helper.InicializarGridReadObly(radGridView1, new[] { "preciodesc", });
            radGridView1.UserAddedRow += RadGridView1_UserAddedRow;
            radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
            this.SizeChanged += FacturacionRapida_SizeChanged;
            radGridView1.TableElement.RowHeight = Properties.Resources.trash.Height;
            radGridView1.CellClick += RadGridView1_CellClick;
            radGridView1.UserDeletingRow += RadGridView1_UserDeletingRow;
         

            this.WindowState = FormWindowState.Maximized;
        }

       

        private void RadGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var res = MessageManager.SowMessage("Desea eliminar esta configuración?", ThemeName, false, true);
            if (res == DialogResult.Yes)
            {
                BaseGridNavigator baseGridNavigator = (BaseGridNavigator) sender;
                 DataContainer.Instance().dbManager.DeleteConf((facturacionRapidaconf)baseGridNavigator.GridViewElement.CurrentRow.DataBoundItem);
            }
            else
                e.Cancel = true;
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
                    e.Row.Cells["letraFact"].Value = "Factura C";
                    e.Row.Cells["cbteid"].Value = 11;
                    e.Row.Cells["ptovta"].Value = 1;
                }
            }
            if (e.Column.Name.ToLower().Contains("letrafact") || e.Column.Name.ToLower().Contains("conceptodesc"))
            {
                var generic = e.Column.Name.ToLower().Contains("letrafact") ? "cbte" : "conceptos";
                var opciones = DataContainer.Instance().dbManager.GenericTable(generic);
                CreateMenufromList(opciones, e.Row, generic);
            }

        }

        private void CreateMenufromList(IEnumerable<dynamic> opciones, GridViewRowInfo row,string column)
        {
            List<MenuItem> menus = new List<MenuItem>();
            foreach (var opcion in opciones)
            {
                MenuItem m = new MenuItem
                {
                    Name = opcion.Id.ToString(),
                    Text = opcion.Desc,
                    Tag = column
                };

                m.Click += M_Click;
                menus.Add(m);
            }
            ContextMenu c = new ContextMenu(menus.ToArray());
            _row = row;
            c.Show(this, MousePosition);
        }

       

        private GridViewRowInfo _row;
        private void M_Click(object sender, EventArgs e)
        {
             MenuItem m=(MenuItem)sender;
             switch (m.Tag.ToString())
             {
                case "cbte":
                    _row.Cells["cbteId"].Value = m.Name;
                    _row.Cells["letraFact"].Value = m.Text;
                    break;
                case "conceptos":
                    _row.Cells["conceptoid"].Value = m.Name;
                    _row.Cells["conceptoDesc"].Value = m.Text;
                    break;
            }

            
            DataContainer.Instance().dbManager.EditCOnfRapida((facturacionRapidaconf)_row.DataBoundItem);

        }

        private void CalcularMontos(GridViewRowInfo row) {
            if (row.DataBoundItem != null)
            {
                var detalle = (facturacionRapidaconf)row.DataBoundItem;
                var total = detalle.precio * detalle.cantidad;
                float impuestos = 0;
                if (detalle.ivavalue!=null && detalle.ivavalue > 0)
                {
                    impuestos = (float)(total / (1 + (detalle.ivavalue / 100)));
                    row.Cells["ivavalue"].Value = total - Math.Round(impuestos);
                }
                row.Cells["total"].Value = total;
            }

        }
    }
}
