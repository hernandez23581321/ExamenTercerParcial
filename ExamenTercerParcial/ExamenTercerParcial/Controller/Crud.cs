using ExamenTercerParcial.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ExamenTercerParcial.Controller
{
    public class Crud
    {
        Conexion conn = new Conexion();



        public Task<List<Pagos>> getReadPagos()
        {
            return conn.GetConnectionAsync().Table<Pagos>().ToListAsync();
           
        }
        public  IEnumerable<Pagos> SELECT_WHERE()
        {
            return conn.Conn().Query<Pagos>("SELECT * FROM Pagos order by fecha desc");
        }

        public Task<Pagos> getPagosId(int id)
        {
            return conn
                .GetConnectionAsync()
                .Table<Pagos>()
                .Where(item => item.idPago == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> getPagosUpdateId(Pagos pagos)
        {
            return conn
                .GetConnectionAsync()
                .UpdateAsync(pagos);

        }

        public Task<int> Delete(Pagos pagos)
        {
            return conn
                .GetConnectionAsync()
                .DeleteAsync(pagos);
        }


    }
}
