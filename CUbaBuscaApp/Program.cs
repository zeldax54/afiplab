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
            Application.Run(new Satrt());
           

            //Application.Run(new FacturaForm());

        }
    }
}
