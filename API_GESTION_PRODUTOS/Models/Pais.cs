using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ciudad> Ciudades { get; set; }
    }
}
