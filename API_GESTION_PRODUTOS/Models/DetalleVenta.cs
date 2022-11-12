using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int? VentaId { get; set; }
        [JsonRequired]
        public int ProductoId { get; set; }
        [JsonRequired]
        public int Cantidad { get; set; }
        public float? PrecioVenta { get; set; }
        public float? Total { get; set; }
        [JsonIgnore]
        public Venta? Venta { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }

    }
}
