using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Ajuste
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        [JsonRequired]
        public int EmpleadoId { get; set; }
        public DateTime? Fecha { get; set; }
        [JsonIgnore]
        public Empleado? Empleado { get; set; }
        [JsonIgnore]
        public virtual List<DetalleAjuste>? DetalleAjustes { get; set; }
    }
}
