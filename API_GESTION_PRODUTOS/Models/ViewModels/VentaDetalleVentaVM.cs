namespace API_GESTION_PRODUTOS.Models.ViewModels
{
    public class VentaDetalleVentaVM
    {
        public Venta Venta { get; set; }
        public List<DetalleVenta>? DetalleVentas { get; set; }
    }
}
