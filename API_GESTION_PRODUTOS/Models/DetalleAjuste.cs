using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class DetalleAjuste
    {
        public int Id { get; set; }
        public int? AjusteId { get; set; }
        [JsonRequired]
        public int ProductoId { get; set; }
        [JsonRequired]
        public int Cantidad { get; set; }
        [JsonRequired]
        public TipoMovimento TipoMovimento { get; set; }
        [JsonIgnore]
        public Ajuste? Ajuste { get; set; }
        [JsonIgnore]
        public Producto? Producto { get; set; }
    }

    public enum TipoMovimento { 
        Entrada = 1, 
        Salida = 2
    }

}
