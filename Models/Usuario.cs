using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Rol { get; set; }   // ADMIN o USER
    }
}
