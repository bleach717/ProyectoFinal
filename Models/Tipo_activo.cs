using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class TipoActivo
    {
        public int id_tipo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string ruta_imagen { get; set; }

    }
}