using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class RegistroAlmacen
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
