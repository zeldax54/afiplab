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
    public partial class Homologaciones : RadForm
    {
        
        public Homologaciones(IEnumerable<dynamic> plista)
        {
            InitializeComponent();           
            radGridView1.DataSource = plista;
           
        }

        private void Homologaciones_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
            radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView1.AllowDeleteRow = false;
        }
    }
}
