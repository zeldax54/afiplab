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
    public partial class FacturaForm : RadForm
    {
        public FacturaForm()
        {
            InitializeComponent();
        }

        private void FacturaForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
        }
    }
}
