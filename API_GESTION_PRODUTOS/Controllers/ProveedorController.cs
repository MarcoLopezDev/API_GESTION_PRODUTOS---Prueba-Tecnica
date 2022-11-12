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
    public class ProveedorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProveedorController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Listar todos los Proveedores.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Proveedor> listaProveedores = new List<Proveedor>();

            try
            {
                listaProveedores = _db.Proveedores.Include(p => p.Ciudad).ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaProveedores });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaProveedores });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un proveedor en especifico en funcion del Id.
        /// </summary>
        /// <param name="proveedorId"></param>
        [HttpGet]
        [Route("Buscar/{proveedorId:int}")]
        public ActionResult Buscar(int proveedorId)
        {
            Proveedor oProveedor = _db.Proveedores.Find(proveedorId);

            if (oProveedor == null)
            {
                return BadRequest("Proveedor no encontrado");
            }

            try
            {
                oProveedor = _db.Proveedores.Include(p => p.Ciudad).Where(p => p.Id == proveedorId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oProveedor });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oProveedor });
                throw;
            }
        }

        /// <summary>
        /// Registrar un Proveedor.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Proveedor
        ///     {
        ///        "nombre": "string",
        ///        "direccion": "string",
        ///        "ciudadId": 0,,
        ///        "correo": "string",
        ///        "telefono": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Proveedor proveedor)
        {

            try
            {
                _db.Proveedores.Add(proveedor);
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
        /// Modificar un Proveedor.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Modificar - Proveedor
        ///     {
        ///        "id": 0,
        ///        "nombre": "string",
        ///        "direccion": "string",
        ///        "ciudadId": 0,,
        ///        "correo": "string",
        ///        "telefono": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [Route("Modificar")]
        public IActionResult Modificar([FromBody] UpdateProveedorModel updateProveedorModel)
        {
            Proveedor oProveedor = _db.Proveedores.Find(updateProveedorModel.Id);

            if (oProveedor == null)
            {
                return BadRequest("Proveedor no encontrado");
            }

            try
            {
                oProveedor.Nombre = updateProveedorModel.Nombre is null ? oProveedor.Nombre : updateProveedorModel.Nombre;
                oProveedor.CiudadId = updateProveedorModel.CiudadId is null ? oProveedor.CiudadId : updateProveedorModel.CiudadId;
                oProveedor.Direccion = updateProveedorModel.Direccion is null ? oProveedor.Direccion : updateProveedorModel.Direccion;
                oProveedor.Correo = updateProveedorModel.Correo is null ? oProveedor.Correo : updateProveedorModel.Correo;
                oProveedor.Telefono = updateProveedorModel.Telefono is null ? oProveedor.Telefono : updateProveedorModel.Telefono;


                _db.Proveedores.Update(oProveedor);
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
        /// Elimina un proveedor en especifico en funcion del Id. 
        /// </summary>
        /// <param name="proveedorId"></param>
        [HttpDelete]
        [Route("Eliminar/{proveedorId:int}")]
        public IActionResult Eliminar(int proveedorId)
        {
            Proveedor oProveedor = _db.Proveedores.Find(proveedorId);

            if (oProveedor == null)
            {
                return BadRequest("Proveedor no encontrado");
            }

            try
            {

                _db.Proveedores.Remove(oProveedor);
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
