using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ProyectoFinal.Models
{
    public class Activos_1
    {

        [JsonPropertyName("id_activo")]
        public int IdActivo { get; set; }

        [JsonPropertyName("id_tipo")]
        public int IdTipo { get; set; }

        [JsonPropertyName("detalle")]
        public string Detalle { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; }

        [JsonPropertyName("costo")]
        public decimal? Costo { get; set; }

        [JsonPropertyName("ubicacion")]
        public string Ubicacion { get; set; }

        [JsonPropertyName("disponible")]
        public bool Disponible { get; set; }

        [JsonPropertyName("fecha_registro")]
        public DateTime FechaRegistro { get; set; }

        [JsonPropertyName("propietario")]
        public int Propietario { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("apellido")]
        public string Apellido { get; set; }

        [JsonPropertyName("Marca")]
        public string Marca { get; set; }

        [JsonPropertyName("Modelo")]
        public string Modelo { get; set; }

        [JsonPropertyName("Serie")]
        public string Serie { get; set; }

        [JsonPropertyName("Anio")]
        public int? Anio { get; set; }
    }
}
