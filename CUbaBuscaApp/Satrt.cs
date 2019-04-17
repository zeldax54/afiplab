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
    public partial class Satrt : RadForm
    {
        public Satrt()
        {
            InitializeComponent();
        }

        private void Satrt_Load(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger.WriteLog("Aplicacion iniciada");
            BdManager bd = new BdManager();
            DataContainer.Instance().dbManager = bd;
        }

        private void radMenuItem3_Click(object sender, EventArgs e)
        {
            new Configuracion().ShowDialog();
        }
    }
}
