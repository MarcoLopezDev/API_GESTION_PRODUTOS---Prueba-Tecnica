using API_GESTION_PRODUTOS.Models;
using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class CategoriaProducto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<Producto> Productos { get; set; }

    }
}
