using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class ActivoDTO
    {
        public int IdActivo { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Categoria { get; set; }
        public string Estado { get; set; }
        public string Icono { get; set; }
    }
}
