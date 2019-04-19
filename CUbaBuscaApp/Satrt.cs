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
            string tema = DataContainer.Instance().dbManager.ConfigByKey("tema");
            DataContainer.Instance().Themename = tema;            Helper.SetTheme(this.Controls,this);

            radMenuComboItem2.Items.Add("Dark");
            radMenuComboItem2.Items.Add("Light");
            radMenuComboItem2.Items.Add("Metro");
            radMenuComboItem2.Items.Where(a=>a.Text.ToLower().Contains(tema)).First().Selected = true;
            radMenuComboItem2.ComboBoxElement.SelectedIndexChanged += ComboBoxElement_SelectedIndexChanged;


        }

        private void ComboBoxElement_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (e.Position != -1) {
                DataContainer.Instance().Themename = radMenuComboItem2.Items[e.Position].Text;
                Helper.SetTheme(Controls, this);
            }        
        }

       
      

        private void radMenuItem3_Click(object sender, EventArgs e)
        {
            new Configuracion().ShowDialog();
        }

        private void radMenuItem7_Click(object sender, EventArgs e)
        {
            new ServiociosForm().ShowDialog();
        }

        private void radMenuItem8_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItem12_Click(object sender, EventArgs e)
        {
            new Configuracion().ShowDialog();

        }

        private void radMenuItem14_Click(object sender, EventArgs e)
        {
            new ServiociosForm().ShowDialog();
        }

        private void radMenu1_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItem8_Click_1(object sender, EventArgs e)
        {
            
        }

        private void radMenuComboItem1_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItem9_Click(object sender, EventArgs e)
        {

        }

        private void radMenuComboItem2_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItem9_Click_1(object sender, EventArgs e)
        {
            new PreciosForm().ShowDialog();
        }
    }
}
