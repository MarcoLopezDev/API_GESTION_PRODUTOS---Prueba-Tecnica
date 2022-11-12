using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public int CodigoFactura { get; set; }
        [JsonRequired]
        public int ProveedorId { get; set; }
        [JsonRequired]
        public int EmpleadoId { get; set; }
        public DateTime? Fecha { get; set; }
        public float? Total { get; set; }

        [JsonIgnore]
        public Proveedor? Proveedor { get; set; }
        [JsonIgnore]
        public Empleado? Empleado { get; set; }
        [JsonIgnore]
        public virtual List<DetalleCompra>? DetalleCompras { get; set; }
    }
}
