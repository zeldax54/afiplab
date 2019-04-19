//This code was generated by a tool.
//Changes to this file will be lost if the code is regenerated.
// See the blog post here for help on using the generated code: http://erikej.blogspot.dk/2014/10/database-first-with-sqlite-in-universal.html
using SQLite;
using System;

namespace CUbaBuscaApp
{
    public class SQLiteDb
    {
        string _path;
        public SQLiteDb(string path)
        {
            _path = path;
        }
        
         public void Create()
        {
            using (SQLiteConnection db = new SQLiteConnection(_path))
            {
                db.CreateTable<Configuraciones>();
                db.CreateTable<EstadoFactura>();
                db.CreateTable<Factura>();
                db.CreateTable<Logininfo>();
                db.CreateTable<Precio>();
                db.CreateTable<Servicio>();
                db.CreateTable<usuario>();
            }
        }
    }
    public partial class Configuraciones
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        
        [NotNull]
        public String clave { get; set; }
        
        [NotNull]
        public String valor { get; set; }
        
        public String descripcion { get; set; }
        
    }
    
    public partial class EstadoFactura
    {
        [PrimaryKey, AutoIncrement]
        public Int64 id { get; set; }
        
        [NotNull]
        public String descripcion { get; set; }
        
    }
    
    public partial class Factura
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        
        [NotNull]
        public DateTime fechacreacion { get; set; }
        
        [NotNull]
        public Int64 servicioId { get; set; }
        
        [NotNull]
        public Double cantidad { get; set; }
        
        [NotNull]
        public Double precio { get; set; }
        
        [NotNull]
        public Double total { get; set; }
        
        [NotNull]
        public String nombrecliente { get; set; }
        
        [NotNull]
        public String letrafact { get; set; }
        
        [NotNull]
        public Int64 precioId { get; set; }
        
        [NotNull]
        public Int64 estadoId { get; set; }
        
    }
    
    public partial class Logininfo
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        
        [NotNull]
        public String uniqueId { get; set; }
        
        [NotNull]
        public DateTime generationTime { get; set; }
        
        [NotNull]
        public DateTime expirationTime { get; set; }
        
        [NotNull]
        public String sign { get; set; }
        
        [NotNull]
        public String token { get; set; }
        
    }
    
    public partial class Precio
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        
        [NotNull]
        public Int64 ServicioId { get; set; }
        
        [NotNull]
        public Double precio { get; set; }
        
        [NotNull]
        public DateTime vigenciaDesde { get; set; }
        
        [NotNull]
        public DateTime vigenciaHasta { get; set; }
        
        public String descripcion { get; set; }
        
    }
    
    public partial class Servicio
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        
        [Unique(Name = "Servicio_nombre", Order = 0)]
        [NotNull]
        public String nombre { get; set; }
        
    }
    
    public partial class usuario
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        
        [NotNull]
        public String nombre { get; set; }
        
        [NotNull]
        public String email { get; set; }
        
    }
    
}
