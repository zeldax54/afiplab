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

       
    }
}
