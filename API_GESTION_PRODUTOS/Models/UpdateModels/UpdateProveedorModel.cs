namespace API_GESTION_PRODUTOS.Models.UpdateModels
{
    public class UpdateProveedorModel
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public int? CiudadId { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
    }
}
