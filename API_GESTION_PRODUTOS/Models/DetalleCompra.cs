using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int? CompraId { get; set; }
        [JsonRequired]
        public int ProductoId { get; set; }
        [JsonRequired]
        public int Cantidad { get; set; }
        public float? PrecioCompra { get; set; }
        public float? Total { get; set; }
        [JsonIgnore]
        public Compra? Compra { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
