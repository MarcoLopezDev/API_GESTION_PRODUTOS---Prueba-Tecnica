using API_GESTION_PRODUTOS.Data;
using API_GESTION_PRODUTOS.Models;
using API_GESTION_PRODUTOS.Models.ViewModels;
using API_GESTION_PRODUTOS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_GESTION_PRODUTOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        IRegistroAlmacenService registroAlmacenService;

        public CompraController(ApplicationDbContext db, IRegistroAlmacenService registroAlmacen)
        {
            _db = db;
            registroAlmacenService = registroAlmacen;
        }


        /// <summary>
        /// Listar todas las Compras.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Compra> listaCompra = new List<Compra>();

            try
            {
                listaCompra = _db.Compras.Include(p => p.DetalleCompras).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaCompra });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaCompra });
                throw;
            }
        }

        /// <summary>
        /// Deveulve una compra en especifico en funcion del Id.
        /// </summary>
        /// <param name="compraId"></param>
        [HttpGet]
        [Route("Buscar/{compraId:int}")]
        public ActionResult Buscar(int compraId)
        {
            Compra oCompra = _db.Compras.Find(compraId);

            if (oCompra == null)
            {
                return BadRequest("Compra no encontrado");
            }

            try
            {
                oCompra = _db.Compras.Include(p => p.DetalleCompras).Where(p => p.Id == compraId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oCompra });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oCompra });
                throw;
            }
        }


        /// <summary>
        /// Registrar una Compra. Se puede registrar uno o mas productos definidos en el detalleCompra. 
        /// Al hacer una compra se aumenta en el registro de almacen.   
        /// El ejemplo está dado para la compra de 2 productos. Solo se aumenta o quita detalleCompras en funcion a lo requerido.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - CompraDetalleCompra
        ///     {
        ///        "compra": {
        ///             "codigoFactura": 0,
        ///             "proveedorId": 0,
        ///             "empleadoId": 0
        ///        },
        ///        "detalleCompras": [
        ///             {
        ///                 "productoId": 0,
        ///                 "cantidad": 0
        ///             },
        ///             {
        ///                 "productoId": 0,
        ///                 "cantidad": 0
        ///             }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] CompraDetalleCompraVM compraDetalleCompraVM)
        {
            Proveedor proveedor = await _db.Proveedores.FindAsync(compraDetalleCompraVM.Compra.ProveedorId);
            Empleado empleado = await _db.Empleados.FindAsync(compraDetalleCompraVM.Compra.EmpleadoId);
            List<Producto> listaProducto = new List<Producto>();
            foreach (var dc in compraDetalleCompraVM.DetalleCompras)
            {
                Producto producto = await _db.Productos.FindAsync(dc.ProductoId);
                listaProducto.Add(producto);
            }

            if (empleado == null)
            {
                return BadRequest("El empleado de la venta no existe");
            }
            if (proveedor == null)
            {
                return BadRequest("El proveedor de la venta no existe");
            }
            foreach (var producto in listaProducto)
            {
                if (producto == null)
                {
                    return BadRequest("Uno de los productos de la compra no existe");
                }
            }

            Compra oCompra = compraDetalleCompraVM.Compra;
            List<DetalleCompra> listaDetalleCompra = new List<DetalleCompra>();

            Producto oProducto = new Producto();
            float? total = 0;
            float? subtotal;
            
            try
            {
                foreach (DetalleCompra detalleCompra in compraDetalleCompraVM.DetalleCompras)
                {
                    subtotal = 0;
                    oProducto = await _db.Productos.FindAsync(detalleCompra.ProductoId);
                    detalleCompra.PrecioCompra = oProducto.PrecioCompra;
                    subtotal = detalleCompra.PrecioCompra * detalleCompra.Cantidad;
                    detalleCompra.Total = subtotal;
                    total = total + subtotal;

                    listaDetalleCompra.Add(detalleCompra);
                }

                oCompra.Total = total;
                oCompra.Fecha = DateTime.Now;
                oCompra.DetalleCompras = listaDetalleCompra;

                //Registro de la compra y sus detalles
                _db.Compras.Add(oCompra);
                await _db.SaveChangesAsync();
                //Registro de los productos en almacen
                registroAlmacenService.AdicionarProducto(listaDetalleCompra);

                return StatusCode(StatusCodes.Status200OK, new { message = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                throw;
            }
        }

    }
}
