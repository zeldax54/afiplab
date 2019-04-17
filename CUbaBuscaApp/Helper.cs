using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
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

            r.AllowAddNewRow = true;
            r.AllowAddNewRow = true;
            r.AllowDeleteRow = true;
            r.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            if(fillDock)
            r.Dock = System.Windows.Forms.DockStyle.Fill;
            foreach (var colum in columnsBlock)
                r.Columns[colum].ReadOnly = true;
        }

    }
}
