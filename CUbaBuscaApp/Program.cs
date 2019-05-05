using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI.Localization;

namespace CUbaBuscaApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RadGridLocalizationProvider.CurrentProvider = new MyEnglishRadGridLocalizationProvider();
            log4net.Config.XmlConfigurator.Configure();
            Logger.WriteLog("Aplicacion iniciada");
            BdManager bd = new BdManager();
            DataContainer.Instance().dbManager = bd;
            string tema = DataContainer.Instance().dbManager.ConfigByKey("tema");
            DataContainer.Instance().Themename = tema;
              Application.Run(new Satrt());


          //  Application.Run(new PrinterForm());

        }
    }
}
