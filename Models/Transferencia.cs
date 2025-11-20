using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class Transferencia
    {
        public int IdTransferencia { get; set; }
        public int IdActivo { get; set; }
        public int IdUsuarioOrigen { get; set; }
        public int IdUsuarioDestino { get; set; }
        public DateTime FechaTransferencia { get; set; }
    }
}
