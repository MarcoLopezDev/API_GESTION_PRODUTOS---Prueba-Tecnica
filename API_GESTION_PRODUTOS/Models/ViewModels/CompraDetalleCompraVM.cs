namespace API_GESTION_PRODUTOS.Models.ViewModels
{
    public class CompraDetalleCompraVM
    {
        public Compra Compra { get; set; }
        public List<DetalleCompra>? DetalleCompras { get; set; }
    }
}
