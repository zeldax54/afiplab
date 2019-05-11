using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace CUbaBuscaApp
{
    public partial class FacturaForm : RadForm
    {
        private Factura _Factura;
        private IEnumerable<FacturaDetalles> _Detalles;
        public FacturaForm(Factura factura=null,IEnumerable<FacturaDetalles> detalles=null)
        {
            _Factura = factura;
            _Detalles = detalles;
            InitializeComponent();
        }

        private void FacturaForm_Load(object sender, EventArgs e)
        {
            
            //Temporal config
            //BdManager bd = new BdManager();
            //DataContainer.Instance().dbManager = bd;
            //DataContainer.Instance().Themename = DataContainer.Instance().dbManager.ConfigByKey("tema");
            //end temporal
            Helper.SetTheme(this.Controls, this);            
            tipofactcombo.DataSource = DataContainer.Instance().dbManager.GenericTable("cbte");
            monedacombo.DataSource = DataContainer.Instance().dbManager.GenericTable("moneda");         
            estadoCombo.DataSource= DataContainer.Instance().dbManager.GenericTable("estado");
            conceptocombo.DataSource = DataContainer.Instance().dbManager.GenericTable("conceptos");

            clientebombo.DataSource = DataContainer.Instance().dbManager.Clientes();
            tipodocCliente.DataSource = DataContainer.Instance().dbManager.GenericTable("docs");
            formapagocombo.DataSource = DataContainer.Instance().dbManager.GenericTable("formapago");
            respanteivacliente.DataSource= DataContainer.Instance().dbManager.GenericTable("respanteivacliente");

            //ocular cols

            Helper.OcularColumsCombo(new[] { tipofactcombo,
                monedacombo, estadoCombo,
                conceptocombo,clientebombo,tipodocCliente,formapagocombo }, new []{ "Id","id", "FchDesde", "FchHasta","idtipoDoc" });

            setColumsMode(new[] { tipofactcombo, monedacombo }, BestFitColumnMode.DisplayedCells);
            //

            monedacombo.MultiColumnComboBoxElement.TextBoxElement.TextChanging += TextBoxElement_TextChanging;
           // tipofactcombo.MultiColumnComboBoxElement.TextBoxElement.TextChanging += TextBoxElement_TextChanging;

            //detallegrid set
            detallesGrid.DataSource = new List<FacturaDetalles>();
            Helper.InicializarGrid(detallesGrid, new[] { "id", "facturaid","precioid","impuestoid","monedaid" });            
            detallesGrid.EnableFiltering = false;
            detallesGrid.UserAddedRow += DetallesGrid_UserAddedRow;
            foreach (var c in detallesGrid.Columns)
                if (c.Name != "cantidad")
                    c.ReadOnly = true;       

            var eptoveta = DataContainer.Instance().dbManager.ConfigByKey("ptovta");
            this.ptoveta.Text = eptoveta;
            this.Text = "Nueva factura";
            fecfacvDate.Value = DateTime.Now;

           

            if (_Factura != null && _Detalles != null)
            {
                detallesGrid.AllowDeleteRow = false;
                detallesGrid.AllowEditRow = false;

                this.Text = "Edicion de factura idfactura :" + _Factura.Id;
                this.ptoveta.Text = _Factura.ptovta.ToString(); this.ptoveta.ReadOnly = true;
                this.total.Text = _Factura.total.ToString();

                this.conceptocombo.MultiColumnComboBoxElement.SelectedIndex =(int) DataContainer.Instance().dbManager.
                    ConceptoById((long)_Factura.conceptoId).Id-1;
                this.conceptocombo.MultiColumnComboBoxElement.EditorControl.CurrentRowChanging += EditorControl_CurrentRowChanging2;

                tipofactcombo.MultiColumnComboBoxElement.SelectedIndex = (int)DataContainer.Instance().dbManager.
                    TipofactById((long)_Factura.cbteId).Id-1;

               
                this.estadoCombo.MultiColumnComboBoxElement.SelectedIndex = (int)DataContainer.Instance().dbManager.
                    EstadooById((long)_Factura.estadoId).id-1;

                detallesGrid.DataSource = _Detalles;
                this.numerofactura.Text = _Factura.numeroFact.ToString();
                this.caetextbox.Text = _Factura.cae;
                Totalizar();
                /* this.monedacombo.MultiColumnComboBoxElement.SelectedIndex = (int)DataContainer.Instance().dbManager.
                    MonedatById((long)_Factura.monedaId).id - 1;*/

                radButton1.Text = "Anular";
                if (Helper.ReadOnlyCmprob(_Factura)) {
                    radButton1.Hide();    
                   
                }
                tipofactcombo.MultiColumnComboBoxElement.TextBoxElement.TextChanging += TextBoxElement_TextChanging1;
                monedacombo.MultiColumnComboBoxElement.TextBoxElement.TextChanging += TextBoxElement_TextChanging1;
                conceptocombo.MultiColumnComboBoxElement.TextBoxElement.TextChanging += TextBoxElement_TextChanging1;
                estadoCombo.MultiColumnComboBoxElement.TextBoxElement.TextChanging += TextBoxElement_TextChanging1;
                //Cliente
                if (_Factura.clientdId != null)
                {
                    clientebombo.ValueMember = "Id";
                    clientebombo.SelectedValue = DataContainer.Instance().dbManager.GetCliente((long)_Factura.clientdId).id;
                }

              
                else
                 clientebombo.SelectedIndex = -1;
                crearclientebutton.Hide();

            }
            else {

                //select tipo fact defecti
                int idtipofact = int.Parse(DataContainer.Instance().dbManager.ConfigByKey("idtipofactdefault"));
                int idconceptodef = int.Parse(DataContainer.Instance().dbManager.ConfigByKey("idconceptodef"));
                tipofactcombo.MultiColumnComboBoxElement.SelectedIndex = idtipofact - 1;
                conceptocombo.MultiColumnComboBoxElement.SelectedIndex = idconceptodef - 1;
                detallesGrid.CellEndEdit += DetallesGrid_CellEndEdit;
                detallesGrid.UserDeletedRow += DetallesGrid_UserDeletedRow;
                detallesGrid.CellClick += DetallesGrid_CellClick; ;
                radButton1.Text = "Facturar";
                radButton2.Hide();
                clientebombo.SelectedIndex = -1;

            }
            monedacombo.EditorControl.CurrentRowChanging += EditorControl_CurrentRowChanging;
            estadoCombo.EditorControl.CurrentRowChanging += EditorControl_CurrentRowChanging1;
            this.SizeChanged += FacturaForm_SizeChanged;
            this.WindowState = FormWindowState.Maximized;
         
        }

        private void TextBoxElement_TextChanging1(object sender, Telerik.WinControls.TextChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void TextBoxElement_TextChanging(object sender, Telerik.WinControls.TextChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void FacturaForm_SizeChanged(object sender, EventArgs e)
        {
            tabControl1.Width = (int)(Width * 0.8);
            detallesGrid.Width =(int)( Width * 0.8);
        }

        private void EditorControl_CurrentRowChanging2(object sender, CurrentRowChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void EditorControl_CurrentRowChanging1(object sender, CurrentRowChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void EditorControl_CurrentRowChanging(object sender, CurrentRowChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void DetallesGrid_UserDeletedRow(object sender, GridViewRowEventArgs e)
        {
            Totalizar();
        }

        private void DetallesGrid_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
           
              CalcularMontos(e.Row);
        }

        private void DetallesGrid_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
           
                CalcularMontos(e.Row);
        }

        private void DetallesGrid_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name.ToLower().Contains("precio") && e.Value==null)
            {
                var preciosForm = new PreciosForm(fecfacvDate.Value,true);
                preciosForm.ShowDialog();
                var precio = preciosForm.P;
               
                 if (precio != null)
                {
                    if (precio.descripcion.ToLower().Contains("carga manual"))
                    {
                        e.Row.Cells["precio"].ColumnInfo.ReadOnly = false;
                        e.Row.Cells["preciodesc"].ColumnInfo.ReadOnly = false;
                    }
                    e.Row.Cells["precio"].Value = precio.precio;
                    e.Row.Cells["precioId"].Value = precio.Id;
                    e.Row.Cells["impuestoid"].Value = precio.ivaId;
                    e.Row.Cells["impuestobase"].Value = precio.ivamonto;
                    e.Row.Cells["cantidad"].Value = 0;
                    e.Row.Cells["monedaid"].Value = ((Moneda)((GridViewRowInfo)monedacombo.SelectedItem).DataBoundItem).Id;
                    e.Row.Cells["monedadesc"].Value = ((Moneda)((GridViewRowInfo)monedacombo.SelectedItem).DataBoundItem).Desc;
                    e.Row.Cells["total"].Value = 0;
                    //e.Row.Cells["cantidad"].Value = 1;

                    e.Row.Cells["impuestovalor"].Value = 0;
                    e.Row.Cells["preciodesc"].Value = precio.descripcion;
                    e.Row.Cells["serviciodesc"].Value = DataContainer.Instance().dbManager.GetServicio(precio.ServicioId).nombre;

                    Totalizar();
                    // detallesGrid.Rows.Add(e.Row);

                }
                //var imp = ivaform.IvaValue;
                //var id = ivaform.Ivaid;
                //e.Row.Cells[e.ColumnIndex].Value = imp;
                //e.Row.Cells[e.ColumnIndex - 1].Value = id;
            }
        }

       


        private void CalcularMontos(GridViewRowInfo row)
        {
            if (row.DataBoundItem != null) {
                var detalle = (FacturaDetalles)row.DataBoundItem;
                var total = detalle.precio * detalle.cantidad;
                var ivamonto =
              row.Cells["total"].Value = total;
                float impuestos = 0;
                if (detalle.impuestobase > 0)
                {
                    impuestos = (float)(total / (1 + (detalle.impuestobase / 100)));
                    row.Cells["impuestovalor"].Value = total - Math.Round(impuestos);
                }

                //totales
                Totalizar();

            }  

        }

        private void Totalizar() {

            float totalfactura = 0;
            float totalimp = 0;
            //
            float totalgravado = 0;
            float totalNogravado = 0;
            foreach (var gridViewRowInfo in detallesGrid.Rows)
            {
                var detallefact = (FacturaDetalles)gridViewRowInfo.DataBoundItem;
                totalfactura += (float)(detallefact.total??0);
                totalimp += (float)(detallefact.impuestovalor??0);
            }
            totalNogravado = totalfactura - totalimp;
            totalgravado = totalfactura - totalNogravado;
            this.total.Text = totalfactura.ToString();
            totalcimpuestos.Text = totalimp.ToString();
            this.gravado.Text = totalgravado.ToString();
            this.nogravado.Text = totalNogravado.ToString();
        }

        private void setColumsMode(RadMultiColumnComboBox[] combo, BestFitColumnMode mode) {
            foreach (var c in combo) {
                c.MultiColumnComboBoxElement.DropDownWidth = c.Width;
                c.AutoSizeDropDownToBestFit = false;
                c.BestFitColumns(true, false);
            }

        }


      




        private void Facturar() {

            if (detallesGrid.RowCount == 0)
            {
                MessageManager.SowMessage("No hay detalles para facturar", ThemeName);
                return;
            }
            if(float.Parse(DataContainer.Instance().dbManager.ConfigByKey("maxmontonocliente")) <= double.Parse(total.Text) && clientebombo.SelectedIndex==-1)
            {
                MessageManager.SowMessage($"Para facturas mayores o iguales a ${DataContainer.Instance().dbManager.ConfigByKey("maxmontonocliente")} es necesario indicar el" +
                                          $" nombre del cliente", ThemeName);return;
            }
            DialogResult res = MessageManager.SowMessage("Se enviara esta factura a la Afip comprobante con fecha " + fecfacvDate.Value.ToString("dd-MM-yyyy"), ThemeName, false, true);
                if (res == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    Factura f = new Factura();
                    f.clientdId = null;
                    f.conceptoId = ((afipService.ConceptoTipo)((GridViewDataRowInfo)conceptocombo.SelectedItem).DataBoundItem).Id;
                    f.letrafact = ((afipService.CbteTipo)((GridViewDataRowInfo)tipofactcombo.SelectedItem).DataBoundItem).Desc;
                    f.cbteId = ((afipService.CbteTipo)((GridViewDataRowInfo)tipofactcombo.SelectedItem).DataBoundItem).Id;
                    f.monedaId = ((Moneda)((GridViewDataRowInfo)monedacombo.SelectedItem).DataBoundItem).Id;
                    f.monedadesc = ((Moneda)((GridViewDataRowInfo)monedacombo.SelectedItem).DataBoundItem).Desc;
                    f.total = double.Parse(total.Text);
                    f.fechacreacion = DateTime.Now;
                    f.fechafacturacion = fecfacvDate.Value;
                    f.noGravado = double.Parse(this.nogravado.Text);
                    f.estadoId = ((EstadoFactura)((GridViewDataRowInfo)estadoCombo.SelectedItem).DataBoundItem).id;
                    f.estadodesc = ((EstadoFactura)((GridViewDataRowInfo)estadoCombo.SelectedItem).DataBoundItem).descripcion;
                    int ptovta = int.Parse(this.ptoveta.Text);
                    f.ptovta = ptovta;
                    f.formapagoId = ((formaspago) ((GridViewDataRowInfo) formapagocombo.SelectedItem).DataBoundItem).id;
                    if (clientebombo.SelectedIndex != -1)
                        f.clientdId = ((Clientes) ((GridViewDataRowInfo) clientebombo.SelectedItem).DataBoundItem).id;
                    try
                    {
                        f.Id = DataContainer.Instance().dbManager.SaveFact(f);
                        IEnumerable<FacturaDetalles> detalles = (IEnumerable<FacturaDetalles>)detallesGrid.DataSource;
                        foreach (var d in detalles)
                            d.facturaId = f.Id;
                        DataContainer.Instance().dbManager.SaveFactDet(detalles);
                        afipService.CbteTipo cbte = ((afipService.CbteTipo)((GridViewDataRowInfo)tipofactcombo.SelectedItem).DataBoundItem);

                        WsManager.Facturar(f, detalles, cbte, ptovta, false);
                        MessageManager.SowMessage("Fatura enviada a la AFIP!!!", ThemeName);
                        Cursor = Cursors.Default;
                        Close();
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageManager.SowMessage(ex.Message, ThemeName);
                    }
                }
           
        }

        private void NotaCredito() {
            if (_Factura.estadoId == 2)
            {
              
                DialogResult res = MessageManager.SowMessage("Se enviara la anulación de esta factura a la Afip con fecha "+ fecfacvDate.Value.ToString("dd-MM-yyyy")+".", ThemeName, false, true);
                if (res == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;
                    int inverso = DataContainer.Instance().dbManager.idInverso((int)_Factura.cbteId);
                    var cbte = DataContainer.Instance().dbManager.TipofactById(inverso);
                    Factura nC = new Factura();

                    nC.cbteId = inverso;
                    nC.letrafact = cbte.Desc;
                    nC.Id = 0;
                    nC.cae = null;
                    nC.estadoId = 1;
                    nC.fechacreacion = DateTime.Now;

                    nC.originalidfact = _Factura.Id;
                    nC.total = _Factura.total;
                    nC.monedaId = _Factura.monedaId;
                    nC.monedadesc = _Factura.monedadesc;
                    nC.nombrecliente = _Factura.nombrecliente;
                    nC.ptovta = _Factura.ptovta;
                    nC.conceptoId = _Factura.conceptoId;
                    nC.noGravado = _Factura.noGravado;
                    nC.nombrecliente = _Factura.nombrecliente;
                    nC.NroRef = _Factura.NroRef;
                    nC.fechafacturacion = fecfacvDate.Value;
                    nC.formapagoId = _Factura.formapagoId;
                    nC.clientdId = _Factura.clientdId;
                    var ncdetalles = _Detalles;
                    try
                    {
                        nC.Id = DataContainer.Instance().dbManager.SaveFact(nC);
                        foreach (var d in ncdetalles)
                            d.facturaId = nC.Id;
                        DataContainer.Instance().dbManager.SaveFactDet(ncdetalles);

                        var fact = WsManager.Facturar(nC, ncdetalles, cbte, (int)nC.ptovta, true, (long)_Factura.numeroFact);
                        if (fact.estadoId == 2)
                        {
                            _Factura.estadoId = 5;

                            DataContainer.Instance().dbManager.WriteFactura(_Factura, _Detalles);
                            MessageManager.SowMessage("Nota de credito enviada a la AFIP!!!", ThemeName);
                            Cursor = Cursors.Default;
                            Close();
                        }

                        else
                        {
                            MessageManager.SowMessage("Error anulando con exito. Consultar Log", ThemeName);
                            Cursor = Cursors.Default;
                            Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageManager.SowMessage(ex.Message, ThemeName);
                    }
                }

            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (_Factura != null)
                NotaCredito();
            else
                Facturar();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            new PrinterForm(_Factura, _Detalles).ShowDialog();
        }

        private void crearclientebutton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(nombreCliente.Text) || string.IsNullOrWhiteSpace(nrodoccliente.Text))
            {
                MessageManager.SowMessage("El nombre y numero de documento del cliente son requeridos", ThemeName);return;
            }

            if (tipodocCliente.SelectedIndex == -1 || respanteivacliente.SelectedIndex == -1)
            {
                MessageManager.SowMessage("El tipo de documento y la responsabilidad ante IVA del cliente son requeridos", ThemeName); return;
            }

            long nuber = 0;
            if (!long.TryParse(nrodoccliente.Text, out nuber))
            {
                MessageManager.SowMessage("El numero del cliente debe ser un dato numerico", ThemeName); return;
            }

            Clientes c = new Clientes()
            {
                nrodoc = nuber,
                idtipoDoc=((afipService.DocTipo)((GridViewDataRowInfo)tipodocCliente.SelectedItem).DataBoundItem).Id,
                Nombre = nombreCliente.Text,
                conceptiva = ((Responsabilidades)((GridViewDataRowInfo)respanteivacliente.SelectedItem).DataBoundItem).responsabilidad,
                desctipodoc = ((afipService.DocTipo)((GridViewDataRowInfo)tipodocCliente.SelectedItem).DataBoundItem).Desc,
                docmicilio = domiciliocliente.Text
            };

            var cExiste = DataContainer.Instance().dbManager.ExisteCliente(c);
            if (cExiste != null)
            {
                MessageManager.SowMessage("Ya existe un cliente cargado con ese numero y tipo de documento", ThemeName);
                clientebombo.SelectedItem = cExiste;
            }
            else
            {
                c.id=DataContainer.Instance().dbManager.AddCliente(c);
                clientebombo.DataSource= DataContainer.Instance().dbManager.Clientes();
                clientebombo.ValueMember = "Id";
                clientebombo.SelectedValue =c.id;
            }


        }
    }
}
