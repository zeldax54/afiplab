﻿using System;
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
    public partial class Configuracion : RadForm
    {
        public Configuracion()
        {
            InitializeComponent();
        }

        
        private void Configuracion_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls,this);
            radGridView1.DataSource= DataContainer.Instance().dbManager.ConfiguracionesList();
            Helper.InicializarGrid(radGridView1, new[] { "Id" });
            radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
            radGridView1.UserAddedRow += RadGridView1_UserAddedRow;
            radGridView1.UserDeletingRow += RadGridView1_UserDeletingRow;
        }

        private void RadGridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            var rows = radGridView1.SelectedRows;
            foreach(GridViewDataRowInfo row in rows)
                DataContainer.Instance().dbManager.ManageConfig((Configuraciones)row.DataBoundItem,true);
        }

        

        private void RadGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            DataContainer.Instance().dbManager.ManageConfig((Configuraciones)e.Row.DataBoundItem);
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {            
            if (e.Row.DataBoundItem != null) 
                DataContainer.Instance().dbManager.ManageConfig((Configuraciones)e.Row.DataBoundItem);
        }

       
    }
}
