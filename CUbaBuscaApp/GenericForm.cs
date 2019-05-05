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
    public partial class GenericForm : RadForm
    {
        public GenericForm()
        {
            InitializeComponent();
        }

        private void GenericForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
        }
    }
}
