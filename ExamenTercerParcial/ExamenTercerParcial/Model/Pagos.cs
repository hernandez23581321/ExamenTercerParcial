using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamenTercerParcial.Model
{
    public class Pagos
    {
        [PrimaryKey, AutoIncrement]
        public int idPago { get; set; }
        [MaxLength(250)]
        public string descripcion { get; set; }
        [MaxLength(250)]
        public double monto { get; set; }
        public DateTime fecha { get; set; }
        [MaxLength(100), Unique]
        public byte[] photo_recibo { get; set; }
      

    }
}
