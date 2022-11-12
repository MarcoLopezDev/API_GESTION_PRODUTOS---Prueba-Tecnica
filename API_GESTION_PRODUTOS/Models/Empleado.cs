using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public int? CodigoEmpleado { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime? FechaInicio { get; set; }
        public string? Sucursal { get; set; }
        public string CI { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        [JsonIgnore]
        public virtual ICollection<Venta>? Ventas { get; set; }
        [JsonIgnore]
        public virtual ICollection<Compra>? Compras { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ajuste>? Ajustes { get; set; }
    }
}
