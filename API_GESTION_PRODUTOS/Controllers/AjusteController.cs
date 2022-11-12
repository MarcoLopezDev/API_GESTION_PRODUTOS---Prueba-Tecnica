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
    public class AjusteController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        IRegistroAlmacenService registroAlmacenService;

        public AjusteController(ApplicationDbContext db, IRegistroAlmacenService registroAlmacen)
        {
            _db = db;
            registroAlmacenService = registroAlmacen;
        }

        /// <summary>
        /// Listar todos los Ajustes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Ajuste> listaAjuste = new List<Ajuste>();

            try
            {
                listaAjuste = _db.Ajustes.Include(p => p.DetalleAjustes).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaAjuste });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaAjuste });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un ajuste en especifico en funcion del Id.
        /// </summary>
        /// <param name="ajusteId"></param>
        [HttpGet]
        [Route("Buscar/{ajusteId:int}")]
        public ActionResult Buscar(int ajusteId)
        {
            Ajuste oAjuste = _db.Ajustes.Find(ajusteId);

            if (oAjuste == null)
            {
                return BadRequest("Ajuste no encontrado");
            }

            try
            {
                oAjuste = _db.Ajustes.Include(p => p.DetalleAjustes).Where(p => p.Id == ajusteId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oAjuste });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oAjuste });
                throw;
            }
        }


        /// <summary>
        /// Registrar un Ajuste. Se puede registrar uno o mas ajuste de productos definidos en el detalleAjuste. 
        /// Un ajuste puede ser para aumentar o restar producto en el almacen.   
        /// El ejemplo está dado para el ajuste de 2 productos. Solo se aumenta o quita detalleCompras en funcion a lo requerido.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - AjusteDetalleAjuste (Entrada -> TipoMovimiento = 1, Salida -> TipoMovimiento = 2)
        ///     {
        ///        "ajuste": {
        ///             "descripcion": "string",
        ///             "empleadoId": 0
        ///        },
        ///        "detalleAjustes": [
        ///             {
        ///                 "productoId": 0,
        ///                 "cantidad": 0,
        ///                 "tipoMovimento": 1
        ///             },
        ///             {
        ///                 "productoId": 0,
        ///                 "cantidad": 0,
        ///                 "tipoMovimento": 2
        ///             }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] AjusteDetalleAjusteVM ajusteDetalleAjusteVM)
        {
            //Validaciones
            Empleado empleado = await _db.Empleados.FindAsync(ajusteDetalleAjusteVM.Ajuste.EmpleadoId);
            List<Producto> listaProducto = new List<Producto>();
            foreach (var da in ajusteDetalleAjusteVM.DetalleAjustes)
            {
                Producto producto = await _db.Productos.FindAsync(da.ProductoId);
                listaProducto.Add(producto);
            }

            if (empleado == null)
            {
                return BadRequest("El empleado del ajuste no existe");
            }
            foreach (var producto in listaProducto)
            {
                if (producto == null)
                {
                    return BadRequest("Uno de los productos del ajuste no existe");
                }
            }
            //Accion
            Ajuste oAjuste = ajusteDetalleAjusteVM.Ajuste;
            List<DetalleAjuste> listaDetalleAjuste = new List<DetalleAjuste>();

            Producto oProducto = new Producto();
            RegistroAlmacen oRegistroAlmacen = new RegistroAlmacen();

            try
            {
                foreach (DetalleAjuste detalleAjuste in ajusteDetalleAjusteVM.DetalleAjustes)
                {
                    oProducto = await _db.Productos.FindAsync(detalleAjuste.ProductoId);
                    oRegistroAlmacen = await _db.RegistroAlmacenes.FindAsync(oProducto.Id);

                    if (oRegistroAlmacen.Cantidad < detalleAjuste.Cantidad && detalleAjuste.TipoMovimento == TipoMovimento.Salida)
                    {
                        return BadRequest($"No se tiene suficientes items del producto {oProducto.Descripcion} para ajustar una salida");
                    }
                    listaDetalleAjuste.Add(detalleAjuste);
                }

                oAjuste.Fecha = DateTime.Now;
                oAjuste.DetalleAjustes = listaDetalleAjuste;

                _db.Ajustes.Add(oAjuste);
                await _db.SaveChangesAsync();
                //Ajustando productos del almacen
                registroAlmacenService.AjustarProducto(listaDetalleAjuste);

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
