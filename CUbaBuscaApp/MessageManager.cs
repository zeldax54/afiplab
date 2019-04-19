using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;

namespace CUbaBuscaApp
{
    public static class MessageManager
    {


        public static DialogResult SowMessage(string message,string themename, bool iserror = false,bool isconfirm=false) {
            var icon = iserror ? RadMessageIcon.Error : RadMessageIcon.Info;
            var bootons = isconfirm ? MessageBoxButtons.YesNo : MessageBoxButtons.OK;
            RadMessageBox.SetThemeName(themename);
            return RadMessageBox.Show(message, "Info", bootons,icon);
        }


        
    }
}
