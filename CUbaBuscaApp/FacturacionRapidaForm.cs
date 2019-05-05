using CUbaBuscaApp.Properties;
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
    public partial class FacturacionRapidaForm : RadForm
    {
        public FacturacionRapidaForm()
        {
            InitializeComponent();
        }

        private void FacturacionRapidaForm_Load(object sender, EventArgs e)
        {
            Helper.SetTheme(this.Controls, this);
            radGridView1.DataSource = DataContainer.Instance().dbManager.ListaFacR();
            Helper.InicializarGridReadObly(radGridView1, new[] { "id","servicioid", "percioId","ivaid" });
            radGridView1.EnableFiltering = false;
            this.SizeChanged += FacturacionRapidaForm_SizeChanged;
            this.WindowState = FormWindowState.Maximized;
            //radGridView1.Columns.Add("Facturar", "Facturar");
            radGridView1.CellFormatting += RadGridView1_CellFormatting;
            radGridView1.Columns.Add(new GridViewCommandColumn
            {
                HeaderText = "Facturar",
                Name = "Facturar"
            });
            radGridView1.CellClick += RadGridView1_CellClick;
            
        }

        private void RadGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name == "Facturar")
            {     try
                {
                    facturacionRapidaconf fR = (facturacionRapidaconf)e.Row.DataBoundItem;
                   
                 var res = MessageManager.SowMessage("Se enviara una factura a la afip por " + fR.total, ThemeName, false, true);
                 if (res == DialogResult.Yes)
                 {
                        int ptoveta = (int)fR.ptovta;
                        afipService.CbteTipo cbte = DataContainer.Instance().dbManager.TipofactById((long)fR.cbteid);

                        Factura f = new Factura() {
                            fechacreacion = DateTime.Now,
                            fechafacturacion = DateTime.Now,
                            cbteId = cbte.Id,
                            clientdId = null,
                            conceptoId = fR.conceptoid,
                            letrafact = fR.letraFact,
                            ptovta = ptoveta,
                            estadoId=1,
                            total=(double)fR.total,
                            monedadesc= "Pesos Argentinos",
                            monedaId="PES",
                            noGravado=fR.total-(fR.ivavalue ?? 0),
                        };
                       List< FacturaDetalles> detalles = new List< FacturaDetalles>() { new FacturaDetalles(){
                            impuestobase = fR.porciva,
                            impuestovalor = fR.ivavalue,
                            monedadesc= "Pesos Argentinos",
                            cantidad=fR.cantidad,
                            monedaId="PES",
                            total=fR.total,
                            precio=fR.precio,
                            preciodesc=fR.preciodesc,
                            serviciodesc=fR.frdesc,
                            precioId=fR.percioId,
                            impuestoId=fR.ivaId
                       }

                        };

                        f.Id = DataContainer.Instance().dbManager.SaveFact(f);
                      
                        foreach (var d in detalles)
                            d.facturaId = f.Id;
                        DataContainer.Instance().dbManager.SaveFactDet(detalles);                   

                        WsManager.Facturar(f, detalles, cbte, ptoveta, false);
                        MessageManager.SowMessage("Fatura enviada a la AFIP!!!", ThemeName);

                    }
                }

                catch (Exception ex) {

                    MessageManager.SowMessage(ex.Message, ThemeName);
                }
                
            }
        }

        private void RadGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.Column.Name == "Facturar") {
                GridCommandCellElement cmdCell = e.CellElement as GridCommandCellElement;
                cmdCell.CommandButton.ImageAlignment = ContentAlignment.MiddleCenter;
                cmdCell.CommandButton.Image = Properties.Resources.msoffice_Footer1;
              
            }
        }

     

        private void FacturacionRapidaForm_SizeChanged(object sender, EventArgs e)
        {
            radGridView1.Width = (int)(Width * 0.8);
            radGridView1.Height = (int)(Width * 0.8);

        }
    }
}
