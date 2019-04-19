using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CUbaBuscaApp
{
    class DataContainer
    {
        //Singleton para nunca olvidar
      /*  En ingeniería de software, singleton o instancia única es un patrón de diseño que permite restringir
        la creación de objetos pertenecientes a una clase o el valor de un tipo a un único objeto.
     Su intención consiste en garantizar que una clase solo tenga una instancia y proporcionar un punto de 
     acceso global a ella.El patrón singleton se implementa creando en nuestra clase un método que crea una 
     instancia del objeto solo si todavía no existe alguna.Para asegurar que la clase no puede ser instanciada 
     nuevamente se regula el alcance del constructor(con modificadores de acceso como protegido o privado). */

        private static DataContainer oInstance;
        public BdManager dbManager;
        public string Themename;
        protected DataContainer()
        {
        }

        public static DataContainer Instance()
        {
            if (oInstance == null)
                oInstance = new DataContainer();
            return oInstance;
        }

    }
}
