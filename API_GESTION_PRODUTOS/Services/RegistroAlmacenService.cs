using API_GESTION_PRODUTOS.Data;
using API_GESTION_PRODUTOS.Models;

namespace API_GESTION_PRODUTOS.Services
{
    public class RegistroAlmacenService : IRegistroAlmacenService
    {
        private readonly ApplicationDbContext _db;

        public RegistroAlmacenService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AdicionarProducto(List<DetalleCompra> listDetalleCompra)
        {
            RegistroAlmacen oRegistroAlmacen = new RegistroAlmacen();

            foreach (DetalleCompra detalleCompra in listDetalleCompra)
            {
                oRegistroAlmacen = _db.RegistroAlmacenes.Find(detalleCompra.ProductoId);               
                oRegistroAlmacen.Cantidad = oRegistroAlmacen.Cantidad + detalleCompra.Cantidad;
                _db.RegistroAlmacenes.Update(oRegistroAlmacen);
                _db.SaveChanges();
            }

        }

        public void RestarProducto(List<DetalleVenta> listDetalleVenta)
        {
            RegistroAlmacen oRegistroAlmacen = new RegistroAlmacen();

            foreach (DetalleVenta detalleVenta in listDetalleVenta)
            {
                oRegistroAlmacen = _db.RegistroAlmacenes.Find(detalleVenta.ProductoId);
                oRegistroAlmacen.Cantidad = oRegistroAlmacen.Cantidad - detalleVenta.Cantidad;
                _db.RegistroAlmacenes.Update(oRegistroAlmacen);
                _db.SaveChanges();
            }

        }

        public void AjustarProducto(List<DetalleAjuste> listDetalleAjuste)
        {
            RegistroAlmacen oRegistroAlmacen = new RegistroAlmacen();

            foreach (DetalleAjuste detalleAjuste in listDetalleAjuste)
            {
                if (detalleAjuste.TipoMovimento == TipoMovimento.Salida)
                {
                    oRegistroAlmacen = _db.RegistroAlmacenes.Find(detalleAjuste.ProductoId);
                    oRegistroAlmacen.Cantidad = oRegistroAlmacen.Cantidad - detalleAjuste.Cantidad;
                    _db.RegistroAlmacenes.Update(oRegistroAlmacen);
                    _db.SaveChanges();
                }
                else if(detalleAjuste.TipoMovimento == TipoMovimento.Entrada)
                {
                    oRegistroAlmacen = _db.RegistroAlmacenes.Find(detalleAjuste.ProductoId);
                    oRegistroAlmacen.Cantidad = oRegistroAlmacen.Cantidad + detalleAjuste.Cantidad;
                    _db.RegistroAlmacenes.Update(oRegistroAlmacen);
                    _db.SaveChanges();
                }
            }

        }

    }

    public interface IRegistroAlmacenService {
        void AdicionarProducto(List<DetalleCompra> listDetalleCompra);
        void RestarProducto(List<DetalleVenta> listDetalleVenta);
        void AjustarProducto(List<DetalleAjuste> listDetalleAjuste);
    }
}
