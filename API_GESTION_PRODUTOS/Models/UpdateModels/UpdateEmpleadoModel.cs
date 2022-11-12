using System.ComponentModel.DataAnnotations;

namespace API_GESTION_PRODUTOS.Models.UpdateModels
{
    public class UpdateEmpleadoModel
    {
        public int Id { get; set; }
        public int? CodigoEmpleado { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public DateTime? FechaInicio { get; set; }
        public string? Sucursal { get; set; }
        public string? CI { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}
