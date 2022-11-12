using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public int CodigoProducto { get; set; }
        public string? Descripcion { get; set; }
        public float PrecioVenta { get; set; }
        public float PrecioCompra { get; set; }
        public int CategoriaProductoId { get; set; }
        [JsonIgnore]
        public CategoriaProducto? CategoriaProducto { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetalleVenta>? DetalleVentas { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetalleCompra>? DetalleCompras { get; set; }
        [JsonIgnore]
        public virtual ICollection<DetalleAjuste>? DetalleAjustes { get; set; }
        [JsonIgnore]
        public virtual ICollection<RegistroAlmacen>? RegistroAlmacenes { get; set; }

    }
}
