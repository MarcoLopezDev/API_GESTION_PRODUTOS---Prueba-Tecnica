using API_GESTION_PRODUTOS.Data;
using API_GESTION_PRODUTOS.Models;
using API_GESTION_PRODUTOS.Models.ViewModels;
using API_GESTION_PRODUTOS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_GESTION_PRODUTOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        IRegistroAlmacenService registroAlmacenService;

        public VentaController(ApplicationDbContext db, IRegistroAlmacenService registroAlmacen)
        {
            _db = db;
            registroAlmacenService = registroAlmacen;
        }

        /// <summary>
        /// Listar todos las Ventas.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Venta> listaVenta = new List<Venta>();

            try
            {
                listaVenta = _db.Ventas.Include(p=>p.DetalleVentas).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaVenta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaVenta });
                throw;
            }
        }



        /// <summary>
        /// Deveulve una venta en especifico en funcion del Id.
        /// </summary>
        /// <param name="ventaId"></param>
        [HttpGet]
        [Route("Buscar/{ventaId:int}")]
        public ActionResult Buscar(int ventaId)
        {
            Venta oVenta = _db.Ventas.Find(ventaId);

            if (oVenta == null)
            {
                return BadRequest("Venta no encontrado");
            }

            try
            {
                oVenta = _db.Ventas.Include(p=>p.DetalleVentas).Where(p => p.Id == ventaId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oVenta });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oVenta });
                throw;
            }
        }

        /// <summary>
        /// Registrar una Venta. Se puede registrar uno o mas productos definidos en el detalleVenta. 
        /// Al hacer una venta se disminuye en el registro de almacen.   
        /// El ejemplo está dado para la venta de 2 productos. Solo se aumenta o quita detalleVentas en funcion a lo requerido.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - VentaDetalleVenta
        ///     {
        ///        "venta": {
        ///             "codigoFactura": 0,
        ///             "clienteId": 0,
        ///             "empleadoId": 0
        ///        },
        ///        "detalleVentas": [
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
        public async Task<IActionResult> Registrar([FromBody] VentaDetalleVentaVM ventaDetalleVentaVM)
        {
            //Validaciones
            Empleado empleado = await _db.Empleados.FindAsync(ventaDetalleVentaVM.Venta.EmpleadoId);
            Cliente cliente = await _db.Clientes.FindAsync(ventaDetalleVentaVM.Venta.ClienteId);
            List<Producto> listaProducto = new List<Producto>();
            foreach (var dv in ventaDetalleVentaVM.DetalleVentas)
            {
                Producto producto = await _db.Productos.FindAsync(dv.ProductoId);
                listaProducto.Add(producto);
            }

            if (empleado == null) {
                return BadRequest("El empleado de la venta no existe");
            }
            if (cliente == null)
            {
                return BadRequest("El cliente de la venta no existe");
            }
            foreach (var producto in listaProducto) {
                if (producto == null) {
                    return BadRequest("Uno de los productos de la venta no existe");
                }
            }
            //Accion
            Venta oVenta = ventaDetalleVentaVM.Venta;
            List<DetalleVenta> listaDetalleVenta = new List<DetalleVenta>();

            Producto oProducto = new Producto();
            RegistroAlmacen oRegistroAlmacen = new RegistroAlmacen();
            float? total = 0;
            float? subtotal;
            try
            {
                foreach (DetalleVenta detalleVenta in ventaDetalleVentaVM.DetalleVentas)
                {
                    
                    oProducto = await _db.Productos.FindAsync(detalleVenta.ProductoId);
                    oRegistroAlmacen = await _db.RegistroAlmacenes.FindAsync(oProducto.Id);
                    if ( oRegistroAlmacen.Cantidad < detalleVenta.Cantidad)
                    {
                        return BadRequest($"No se tiene suficientes items del producto {oProducto.Descripcion}");
                    }
                    subtotal = 0;
                    detalleVenta.PrecioVenta = oProducto.PrecioVenta;
                    subtotal = detalleVenta.PrecioVenta * detalleVenta.Cantidad * (float)1.13;
                    detalleVenta.Total = subtotal;
                    total = total + subtotal;

                    listaDetalleVenta.Add(detalleVenta);
                }

                oVenta.Total = total;
                oVenta.Fecha = DateTime.Now;
                oVenta.DetalleVentas = listaDetalleVenta;

                _db.Ventas.Add(oVenta);
                await _db.SaveChangesAsync();
                //Quitando productos del almacen
                registroAlmacenService.RestarProducto(listaDetalleVenta);

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
