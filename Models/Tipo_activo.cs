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
        public string Descripcion { get; set; }
        public int IdImagen { get; set; }
        public string ruta { get; set; }

    }
}