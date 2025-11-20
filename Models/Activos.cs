using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class Activos
    {
        public int IdActivo { get; set; }
        public int IdTipo { get; set; }
        public string Detalle { get; set; }
        public string Estado { get; set; }
        public decimal? Costo { get; set; }
        public string Ubicacion { get; set; }
        public bool Disponible { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Propietario { get; set; }

    }
}
