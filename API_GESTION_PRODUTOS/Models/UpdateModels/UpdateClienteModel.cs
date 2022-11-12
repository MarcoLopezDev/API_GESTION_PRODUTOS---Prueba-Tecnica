﻿using System.Text.Json.Serialization;

namespace API_GESTION_PRODUTOS.Models.UpdateModels
{
    public class UpdateClienteModel
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? CI { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
    }
}
