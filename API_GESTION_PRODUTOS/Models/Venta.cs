using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public int CodigoFactura { get; set; }
        [JsonRequired]
        public int ClienteId { get; set; }
        [JsonRequired]
        public int EmpleadoId { get; set; }        
        public DateTime? Fecha { get; set; }
        public float? Total { get; set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }
        [JsonIgnore]
        public Empleado? Empleado { get; set; }
        [JsonIgnore]
        public virtual List<DetalleVenta>? DetalleVentas { get; set; }
    }
}
