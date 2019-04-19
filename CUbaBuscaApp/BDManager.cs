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

        public static  void InsertarBusqueda()
        {
           
           
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

        public string ManageServicio(Servicio s, bool isdelete = false)
        {
            try
            {
                if (s.Id == null || s.Id == 0)
                    db.Insert(s);
                else
                {
                    if (isdelete) {
                        if (db.Find<Factura>(f => f.servicioId == s.Id) != null) 
                            return "Este servicio esta en uso";                       
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
        public List<Precio> PreciosListByService(long serviceId)
        {
            return db.Table<Precio>().Where(p => p.ServicioId == serviceId).ToList();
        }


        public string EditarPrecio(Precio p) {
            var infacturas = db.Table<Factura>().Where(f => f.precioId == p.Id).Any();
            var originalprecio = db.Find<Precio>(a => a.Id == p.Id);
            if (infacturas && p.precio!=originalprecio.precio) {
                var precioN = new Precio()
                {
                    Id = p.Id,
                    vigenciaDesde = p.vigenciaDesde,
                    vigenciaHasta = p.vigenciaHasta
                };

                db.Update(precioN);
                return "Este precio esta usado en algunas facturas no se puede editar su monto Vigencias actualizadas";
            }
            db.Update(p);
            return "Precio actualizado";
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
                var infacturas = db.Table<Factura>().Where(f => f.precioId == p.Id).Any();
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



    }
}
