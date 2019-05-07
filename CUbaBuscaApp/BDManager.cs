using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace CUbaBuscaApp
{
    public  class BdManager
    {
        string _path;
        SQLiteDb dbManager;
        SQLiteConnection db;
        public BdManager() {

            _path = ConfigurationManager.AppSettings["sqliteUrl"];
             dbManager = new SQLiteDb(_path);
            dbManager.Create();//Innecesario pero hay que hacerlo por la extension importada

            db = new SQLiteConnection(_path);
        }
       

        public List<Configuraciones> ConfiguracionesList() {
            
            return db.Table<Configuraciones>().ToList();
        }

        public string ManageConfig(Configuraciones c,bool isdelete=false) {
            try
            {
                if (c.Id == null || c.Id == 0)
                    db.Insert(c);
                else {                   
                    if (isdelete)
                        db.Delete(c);
                    else
                    db.Update(c);
                }                              
                return "Configuracion actualizada!!";
            }
            catch (Exception e) {
                return e.Message;
            }
        }


        public string ConfigByKey(string clave)
        {
            try
            {
                return db.Find<Configuraciones>(c => c.clave == clave).valor;
            }
            catch (Exception e) {
                throw new Exception("No se ha podido encontrar la key " + clave + " " + e.Message);
            }
        }

        public Configuraciones ConfigObjectByKey(string key) {

            try
            {
                return db.Find<Configuraciones>(c => c.clave == key);
            }
            catch (Exception e)
            {
                throw new Exception("No se ha podido encontrar la key " + key + " " + e.Message);
            }
        }


        //LoginInfo
        public string SaveLogininfo(Logininfo info) {
            try
            {
                int id=db.Insert(info);
                DateTime rango = DateTime.Now.AddDays(-10);
                var logins = db.Table<Logininfo>().Where(a => a.generationTime < rango);
                foreach (var l in logins)
                    db.Delete(l);
                return "ticket salvado " + DateTime.Now;
            }
            catch (Exception e) {
                return e.Message;
            }
        }

        public Logininfo GetLoginInfo()
        {
            try
            {
                var ticket= db.Find<Logininfo>(t => t.expirationTime>DateTime.Now);
                var dif = ticket.expirationTime - DateTime.Now;
                if (dif.TotalMinutes > 10)
                    return ticket;
                return null;
            }
            catch (Exception e)
            {
                Logger.WriteLog("Obteniendo token desde BD " + e.Message);
                return null;
            }
        }


        //Servicios
        public List<Servicio> ServiciosList()
        {
           return db.Table<Servicio>().ToList();
        }

        public Servicio GetServicio(long idservicio)
        {
            return db.Find<Servicio>(idservicio);
        }

        public string ManageServicio(Servicio s, bool isdelete = false)
        {
            try
            {
                if (s.Id == null || s.Id == 0)
                    db.Insert(s);
                else
                {
                    if (isdelete) {
                        //if (db.Find<FacturaDetalles>(f => f.servicioId == s.Id) != null) 
                        //    return "Este servicio esta en uso";                       
                        // todo validar delete de servicio en factura ->precios 
                        db.Delete(s);
                    }                       
                    else
                        db.Update(s);
                }
                return "Servicio actualizado.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //Precios 

        public List<Precio> PreciosList()
        {
            return db.Table<Precio>().ToList();
        }
        public List<Precio> PreciosListByService(long serviceId,DateTime t,bool callfromfact)
        {
                if(callfromfact)
                  return db.Table<Precio>().ToList().Where(p => p.ServicioId == serviceId && p.vigenciaHasta.Date >= t.Date).ToList();
            return db.Table<Precio>().Where(p => p.ServicioId == serviceId).ToList();

        }


        public object[] EditarPrecio(Precio p) {
            var infacturas = db.Table<FacturaDetalles>().Where(f => f.precioId == p.Id).Any();
            var originalprecio = db.Find<Precio>(a => a.Id == p.Id);
            if (infacturas && p.precio!=originalprecio.precio) {                
                return new object[]{ "Este precio esta usado en algunas facturas no se puede editar su monto Vigencias actualizadas" ,false, originalprecio } ;
            }
            db.Update(p);
            return  new object[] { "Precio actualizado",true};

            //var precioN = new Precio()
            //{
            //    Id = p.Id,
            //    vigenciaDesde = p.vigenciaDesde,
            //    vigenciaHasta = p.vigenciaHasta
            //};

            //db.Update(precioN);
        }

        public string AddPrecio(Precio p) {
            try
            {
                var existe= db.Table<Precio>().Where(a => a.precio == p.precio && a.vigenciaDesde==p.vigenciaDesde
                && a.vigenciaHasta==p.vigenciaHasta && a.descripcion==p.descripcion
                ).Any();
                if (existe)
                    return "Un precio con los mismos parametros ya existe";
                db.Insert(p);
                return "Precio insertado";
            }
            catch (Exception e) {
                return e.Message;
            }           
        }
        public string DeletePrecio(Precio p)
        {
            try
            {
                var infacturas = db.Table<FacturaDetalles>().Where(f => f.precioId == p.Id).Any();
                if (infacturas)
                    return "Este precio esta en uso, no se puede eliminar";
                db.Delete<Precio>(p);
                return "Precio Eliminado";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //Generics       

        public void GenericHomolog(dynamic[] objetos,Type t)
        {
            try
            {              
                db.CreateTable(t, CreateFlags.AllImplicit);                
                foreach (var o in objetos)
                {
                    if (db.Find(o.Id,db.GetMapping(t)) != null)
                        db.Update(o,t);
                    else
                        db.Insert(o,t);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<dynamic> GenericTable(string table)
        {
            switch (table) {
            case "moneda":
               return db.Table<Moneda>();
            case "iva":
               return db.Table<IvaTipo>();
            case "conceptos":
               return db.Table<afipService.ConceptoTipo>();
            case "cbte":
               return db.Table<afipService.CbteTipo>();
            case "ptovta":
               return db.Table<afipService.PtoVenta>();
            case "docs":
               return db.Table<afipService.DocTipo>();
            case "estado":
               return db.Table<EstadoFactura>();
            case "formapago":
                  return db.Table<formaspago>();
            case "respanteivacliente":
                 return db.Table<Responsabilidades>();
            }
            return null;
        }


   


        //WriteFacturas
        public void WriteFactura(Factura f,IEnumerable< FacturaDetalles> dets) {
            try
            {
                f.estadodesc = db.Find<EstadoFactura>(f.estadoId).descripcion;
                db.Update(f);
                db.UpdateAll(dets);
            }
            catch (Exception e)
            {
                Logger.WriteLog("Error salvando facturas en BD");
                Logger.WriteLog(e.Message);
                throw new Exception(e.Message);
            }            
        }

        public long SaveFact(Factura f)
        {    
            db.Insert(f);
            return SQLite3.LastInsertRowid(db.Handle);
        }
        public int SaveFactDet(IEnumerable<FacturaDetalles> dets)
        {
           return db.InsertAll(dets);
        }

        public IEnumerable<Factura> FacturasbyFecha(DateTime fecha)
        {
            
            return db.Table<Factura>().ToList().Where(a => a.fechacreacion.Date == fecha.Date).OrderByDescending(a=>a.fechacreacion).ToList();
        }

        public Factura FacturaFromId(int idfactura)
        {
            return db.Table<Factura>().First(a => a.Id == idfactura);
        }

        public IEnumerable<FacturaDetalles> DetallesFromFactura(int idfactura)
        {

            return db.Table<FacturaDetalles>().Where(a => a.facturaId == idfactura).ToList(); ;
        }

        public ConceptoTipo ConceptoById(long idconcepto)
        {
            return db.Find<ConceptoTipo>(idconcepto);
        }

        public EstadoFactura EstadooById(long estadoId)
        {
            return db.Find<EstadoFactura>(estadoId);
        }

        public afipService.CbteTipo TipofactById(long tipoId)
        {
            return db.Find<afipService.CbteTipo>(a=>a.Id==(int)tipoId);
        }

     
        public Moneda MonedatById(long monedaId)
        {
            return db.Find<Moneda>(monedaId);
        }

        public int idInverso(int idcomprob) {
            return (int) db.Table<Inversocomprob>().First(a => a.idcomprob == idcomprob).idinverso;
        }

        //Facturacion Rapida
        public List<facturacionRapidaconf> ListaFacR() {

            return db.Table<facturacionRapidaconf>().ToList();
        }

        public long AddCOnfRapida(facturacionRapidaconf r) {
            try
            {
                db.Insert(r);
                return SQLite3.LastInsertRowid(db.Handle);
            }
            catch (Exception e) {
                throw new Exception(e.Message);
            }           
        }

        public void EditCOnfRapida(facturacionRapidaconf r)
        {
            try
            {
                db.Update(r);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void DeleteConf(facturacionRapidaconf r)
        {
            try
            {
                db.Delete(r);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Listado de clientes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Clientes> Clientes()
        {
            return db.Table<Clientes>().ToList();
        }

        public Clientes GetCliente(long clienteId)
        {
            return db.Find<Clientes>(clienteId);
        }

        public Clientes ExisteCliente(Clientes c)
        {
            return db.Table<Clientes>().FirstOrDefault(a => a.nrodoc == c.nrodoc && a.desctipodoc==c.desctipodoc);
        }

        public long AddCliente(Clientes c)
        {
            try
            {
                db.Insert(c);
                return SQLite3.LastInsertRowid(db.Handle);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

       


    }
}
