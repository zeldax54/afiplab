using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace CUbaBuscaApp
{
    public partial class IvaEditorForm : RadForm
    {
        public IvaEditorForm()
        {
            InitializeComponent();
        }
        public float IvaValue = 0;
        public int Ivaid = 0;

        private void IvaEditorForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
            radGridView1.DataSource = DataContainer.Instance().dbManager.GenericTable("iva");         
            Helper.InicializarGridReadObly(radGridView1, new [] {"id", "FchDesde", "FchHasta" });
            radGridView1.CellDoubleClick += RadGridView1_CellDoubleClick;
          
        }

        private void RadGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            IvaValue = Helper.myparseFloat(e.Value.ToString());
            Ivaid = int.Parse(e.Row.Cells[0].Value.ToString());
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (IvaValue == 0)
            {               
                IvaValue = Helper.myparseFloat( radGridView1.Rows[0].Cells[1].Value.ToString());
                Ivaid = int.Parse(radGridView1.Rows[0].Cells[0].Value.ToString());
            }            
        }

        private void radGridView1_Click(object sender, EventArgs e)
        {

        }
    }
}
