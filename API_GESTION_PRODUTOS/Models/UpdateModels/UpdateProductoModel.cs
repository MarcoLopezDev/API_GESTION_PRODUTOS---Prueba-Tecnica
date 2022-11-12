namespace API_GESTION_PRODUTOS.Models.UpdateModels
{
    public class UpdateProductoModel
    {
        public int Id { get; set; }
        public int? CodigoProducto { get; set; }
        public string? Descripcion { get; set; }
        public float? PrecioVenta { get; set; }
        public float? PrecioCompra { get; set; }
        public int? CategoriaProductoId { get; set; }
    }
}
