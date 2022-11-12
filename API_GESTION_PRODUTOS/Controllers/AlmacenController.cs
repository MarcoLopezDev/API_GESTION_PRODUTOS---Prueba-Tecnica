using API_GESTION_PRODUTOS.Data;
using API_GESTION_PRODUTOS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PRODUTOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlmacenController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public AlmacenController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<RegistroAlmacen> listaRegistroAlmacen = new List<RegistroAlmacen>();

            try
            {
                listaRegistroAlmacen = _db.RegistroAlmacenes.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaRegistroAlmacen});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaRegistroAlmacen});
                throw;
            }
        }

        [HttpGet]
        [Route("Buscar/{productoId:int}")]
        public ActionResult Buscar(int productoId)
        {
            RegistroAlmacen oRegistroAlmacen = _db.RegistroAlmacenes.Find(productoId);

            if (oRegistroAlmacen == null)
            {
                return BadRequest("Producto en almacen no encontrado");
            }

            try
            {
                oRegistroAlmacen = _db.RegistroAlmacenes.Where(p => p.ProductoId == productoId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oRegistroAlmacen});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oRegistroAlmacen});
                throw;
            }
        }

    }
}
