using API_GESTION_PRODUTOS.Data;
using API_GESTION_PRODUTOS.Models;
using API_GESTION_PRODUTOS.Models.UpdateModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_GESTION_PRODUTOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Listar todos los Productos.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Producto> listaProductos = new List<Producto>();

            try
            {
                listaProductos = _db.Productos.Include(p => p.CategoriaProducto).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaProductos });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaProductos });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un producto en especifico en funcion del Id.
        /// </summary>
        /// <param name="productoId"></param>
        [HttpGet]
        [Route("Buscar/{productoId:int}")]
        public ActionResult Buscar(int productoId)
        {
            Producto oProducto = _db.Productos.Find(productoId);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto = _db.Productos.Include(p => p.CategoriaProducto).Where(p => p.Id == productoId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oProducto });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un producto en especifico en funcion del Codigo.
        /// </summary>
        /// <param name="codProducto"></param>
        [HttpGet]
        [Route("BuscarPorCodigo/{codProducto:int}")]
        public ActionResult BuscarPorCodigo(int codProducto)
        {

            try
            {
                Producto oProducto = _db.Productos.Include(p => p.CategoriaProducto).Where(p => p.CodigoProducto == codProducto).FirstOrDefault();

                if (oProducto == null)
                {
                    return BadRequest("Producto no encontrado");
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oProducto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message});
                throw;
            }
        }


        /// <summary>
        /// Registrar un Producto. Al registrar un producto, se crea en un registro en almacen con valor 0 en cantidad.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Producto
        ///     {
        ///        "codigoProducto": 0,
        ///        "descripcion": "string",
        ///        "precioVenta": 0,
        ///        "precioCompra": 0,
        ///        "categoriaProductoId": 0
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Producto producto)
        {
            CategoriaProducto categoriaProducto = _db.CategoriaProductos.Find(producto.CategoriaProductoId);
            if (categoriaProducto == null) {
                return BadRequest("La categoria del producto no existe.");
            }

            try
            {
                _db.Productos.Add(producto);
                _db.SaveChanges();

                RegistroAlmacen registroAlmacen = new RegistroAlmacen();
                registroAlmacen.ProductoId = _db.Productos.OrderBy(p=>p.Id).LastOrDefault().Id;
                registroAlmacen.Cantidad = 0;

                _db.RegistroAlmacenes.Add(registroAlmacen);
                _db.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { message = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                throw;
            }
        }

        /// <summary>
        /// Modificar un Producto.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Producto
        ///     {
        ///        "id": 0,
        ///        "codigoProducto": 0,
        ///        "descripcion": "string",
        ///        "precioVenta": 0,
        ///        "precioCompra": 0,
        ///        "categoriaProductoId": 0
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [Route("Modificar")]
        public IActionResult Modificar([FromBody] UpdateProductoModel updateProductoModel)
        {
            Producto oProducto = _db.Productos.Find(updateProductoModel.Id);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                oProducto.Descripcion = updateProductoModel.Descripcion is null ? oProducto.Descripcion : updateProductoModel.Descripcion;

                _db.Productos.Update(oProducto);
                _db.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                throw;
            }
        }

        /// <summary>
        /// Elimina un producto en especifico en funcion del Id. Al eliminar el Producto el registro en almacen tambien se elimina.
        /// </summary>
        /// <param name="productoId"></param>
        [HttpDelete]
        [Route("Eliminar/{productoId:int}")]
        public IActionResult Eliminar(int productoId)
        {
            Producto oProducto = _db.Productos.Find(productoId);

            if (oProducto == null)
            {
                return BadRequest("Producto no encontrado");
            }

            try
            {
                RegistroAlmacen oRegistroAlmacen = _db.RegistroAlmacenes.Find(productoId);
                _db.Productos.Remove(oProducto);
                _db.RegistroAlmacenes.Remove(oRegistroAlmacen); 
                _db.SaveChanges();
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
