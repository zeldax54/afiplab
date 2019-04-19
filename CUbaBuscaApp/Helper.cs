using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Telerik.WinControls;
using Telerik.WinControls.UI;
namespace CUbaBuscaApp
{
    public static class Helper
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Helper));
        public delegate void ReportarMensaje(string mensaje);

        public static event ReportarMensaje ReportarMensajes;



        public static void DoLog(string mjg)
        {

            _log.Info(DateTime.Now + ":" + mjg);
            ReportarMensajes?.Invoke(mjg);
        }

       

        public static string FindBetweenChars(string texto, string chars = "(")
        {
            try
            {
         
                if (!texto.Contains("("))
                    return String.Empty;
                  var output = texto.Split('(', ')')[1];
               return output;
                
              
            }
            catch (Exception e)
            {

                throw new Exception("Error manejando nombre :" + texto + " " + e.Message);
            }

        }


        public static string RemoveBetweenChars(string paqueteNombre, string chars = "(")
        {
            try
            {
                string fin = paqueteNombre;
                while (fin.Contains(chars))
                {

                    var output = fin.Split('(', ')')[1];
                    fin = fin.Replace($"({output})", "");
                }
                return fin;
            }
            catch (Exception e)
            {

                throw new Exception("Error manejando nombre :" + paqueteNombre + " " + e.Message);
            }

        }

        public static void InicializarGrid(RadGridView r,string[]columnsBlock,bool fillDock=false) {

            foreach (var colum in r.Columns)
                colum.HeaderText = colum.Name.ToUpper();

            r.AllowAddNewRow = true;
            r.AllowAddNewRow = true;
            r.AllowDeleteRow = true;            
            if(fillDock)
            r.Dock = System.Windows.Forms.DockStyle.Fill;           
            foreach (var colum in columnsBlock) {
                r.Columns[colum].ReadOnly = true;
                r.Columns[colum].IsVisible = false;
            }
            r.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            
        }
      

        public static void SetTheme(Control.ControlCollection controls,RadForm formulario)
        {
            var effectiveTheme = String.Empty;
            formulario.Icon = Properties.Resources.Designcontest_Ecommerce_Business_Invoice;
            switch (DataContainer.Instance().Themename.ToLower()) {

                case "dark":
                    effectiveTheme = "VisualStudio2012Dark";
                    SaveThemeConfig("dark");
                    break;
                case "light":
                    effectiveTheme = "VisualStudio2012Light";
                    SaveThemeConfig("light");
                    break;
                case "metro":
                    SaveThemeConfig("metro");
                    effectiveTheme = "TelerikMetro";
                    break;
                default:
                    SaveThemeConfig("metro");
                    effectiveTheme = "TelerikMetro";
                    break;

            }
           
           formulario.ThemeName = effectiveTheme;
            foreach (Control c in controls)
            {
                try
                {
                    
                    (c as RadControl).ThemeName = effectiveTheme;
                }
                catch (Exception e) {
                    Logger.WriteLog("Error cambiando tema " + e.Message);
                }     
            }              
        }

        private static void SaveThemeConfig(string val) {

            var conf = DataContainer.Instance().dbManager.ConfigObjectByKey("tema");
            conf.valor = val;
            DataContainer.Instance().dbManager.ManageConfig(conf);


        }

    }
}
