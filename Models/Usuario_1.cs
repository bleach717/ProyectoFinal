using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoFinal.Models
{
    public class Usuario_1
    {
        /* public int IdUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Rol { get; set; }   // ADMIN o USER*/

        [JsonPropertyName("id_usuario")]
        public int IdUsuario { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }

        [JsonIgnore]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; }

        [JsonPropertyName("imagen")]
        public string Imagen { get; set; } = "usuario.png";
    }
}
