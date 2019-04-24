using System;
using System.Collections.Generic;
using System.Globalization;
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
            r.EnableFiltering = true;
            
        }

        public static void InicializarGridReadObly(RadGridView r, string[] columnsBlock)
        {
            foreach (var colum in r.Columns)
                colum.HeaderText = colum.Name.ToUpper();        
            r.AllowAddNewRow = false;
            r.AllowDeleteRow = false;
            r.AllowEditRow = false;
            r.MultiSelect = false;
            
            if(columnsBlock!=null)
            foreach (var colum in columnsBlock)
            {
                r.Columns[colum].ReadOnly = true;
                r.Columns[colum].IsVisible = false;
            }
            r.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            r.EnableFiltering = true;

        }

        public static void OcularColumsCombo(RadMultiColumnComboBox [] combo,IEnumerable<string> hide)
        {
            foreach (var c in combo)                
                foreach (var col in c.Columns)
                    foreach (var h in hide)
                        if (col.Name == h)
                            col.IsVisible = false;
        }

        public static void HideSomeColums(RadGridView r, string[] columnsBlock) {

            foreach (var colum in columnsBlock)
            {
                r.Columns[colum].ReadOnly = true;
                r.Columns[colum].IsVisible = false;
            }
        }

        public static float myparseFloat(string str) {
            if (string.IsNullOrWhiteSpace(str))
                str = "0";
             str = str.Replace("%", "");
            NumberFormatInfo formatInfo = new NumberFormatInfo();
            formatInfo.CurrencyDecimalSeparator = ".";
            return float.Parse(str, NumberStyles.AllowDecimalPoint, formatInfo);

        }


        public static void SetTheme(Control.ControlCollection controls,RadForm formulario)
        {
            var effectiveTheme = String.Empty;
            formulario.Icon = Properties.Resources.Designcontest_Ecommerce_Business_Invoice;
            string fontName = DataContainer.Instance().dbManager.ConfigByKey("fuente");
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
                case "blue":
                    SaveThemeConfig("blue");
                    effectiveTheme = "TelerikMetroBlue";
                    break;
                 case "inicial":
                    SaveThemeConfig("inicial");
                    effectiveTheme = "ControlDefault";
                    break;
                default:
                    SaveThemeConfig("inicial");
                    effectiveTheme = "ControlDefault";
                    break;

                    

            }
           
           formulario.ThemeName = effectiveTheme;
            foreach (Control c in controls)
            {                
                try
                {
                    string x = c.Name;
                    (c as RadControl).ThemeName = effectiveTheme;                  
                    (c as RadControl).Font = new System.Drawing.Font(fontName, (c as RadControl).Font.Size);
                    foreach (var ch in GetAll(c, typeof(RadTextBox))) {
                        (ch as RadControl).ThemeName = effectiveTheme;
                        (ch as RadControl).Font = new System.Drawing.Font(fontName, (c as RadControl).Font.Size);
                    }
                    foreach (var ch in GetAll(c, typeof(RadLabel)))
                    {
                        (ch as RadControl).ThemeName = effectiveTheme;
                        (ch as RadControl).Font = new System.Drawing.Font(fontName, (c as RadControl).Font.Size);
                    }

                    foreach (var ch in GetAll(c, typeof(RadMultiColumnComboBox)))
                    {
                        (ch as RadControl).ThemeName = effectiveTheme;
                        (ch as RadControl).Font = new System.Drawing.Font(fontName, (c as RadControl).Font.Size);
                    }



                }
                catch (Exception e) {
                    Logger.WriteLog("Error cambiando tema " + e.Message);
                }     
            }              
        }

        public static IEnumerable<Control> GetAll(Control control, Type type)
        {
            var controls = control.Controls.Cast<Control>();

            return controls.SelectMany(ctrl => GetAll(ctrl, type))
                                      .Concat(controls)
                                      .Where(c => c.GetType() == type);
        }

        private static void SaveThemeConfig(string val) {

            var conf = DataContainer.Instance().dbManager.ConfigObjectByKey("tema");
            conf.valor = val;
            DataContainer.Instance().dbManager.ManageConfig(conf);


        }

     

    }
}
