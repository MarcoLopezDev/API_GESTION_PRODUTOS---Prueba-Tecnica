using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Ciudad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int PaisId { get; set; }
        public Pais Pais { get; set; }
        [JsonIgnore]
        public virtual ICollection<Proveedor> Proveedores { get; set; }
    }
}
