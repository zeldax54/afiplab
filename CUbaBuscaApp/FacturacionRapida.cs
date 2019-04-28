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
        }
    }
}
