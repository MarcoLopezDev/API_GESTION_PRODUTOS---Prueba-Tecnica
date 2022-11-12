using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int? CiudadId { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        [JsonIgnore]
        public Ciudad? Ciudad { get; set; }

        [JsonIgnore]
        public virtual ICollection<Compra>? Compras { get; set; }
    }
}
