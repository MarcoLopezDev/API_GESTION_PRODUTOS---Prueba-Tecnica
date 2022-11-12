using API_GESTION_PRODUTOS.Data;
using API_GESTION_PRODUTOS.Models;
using API_GESTION_PRODUTOS.Models.UpdateModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_GESTION_PRODUTOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public EmpleadoController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Listar todos los Empleados.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Empleado> listaEmpleados = new List<Empleado>();

            try
            {
                listaEmpleados = _db.Empleados.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaEmpleados });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaEmpleados });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un empleado en especifico en funcion del Id.
        /// </summary>
        /// <param name="empleadoId"></param>
        [HttpGet]
        [Route("Buscar/{empleadoId:int}")]
        public ActionResult Buscar(int empleadoId)
        {
            Empleado oEmpleado = _db.Empleados.Find(empleadoId);

            if (oEmpleado == null)
            {
                return BadRequest("Empleado no encontrado");
            }

            try
            {
                oEmpleado = _db.Empleados.Where(p => p.Id == empleadoId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oEmpleado });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oEmpleado });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un empleado en especifico en funcion del codigo.
        /// </summary>
        /// <param name="codigoEmpleado"></param>
        [HttpGet]
        [Route("BuscarPorCodigo/{codigoEmpleado:int}")]
        public ActionResult BuscarPorCodigo(int codigoEmpleado)
        {
            try
            {
                Empleado oEmpleado = _db.Empleados.Where(p => p.CodigoEmpleado == codigoEmpleado).FirstOrDefault();

                if (oEmpleado == null)
                {
                    return BadRequest("Empleado no encontrado");
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oEmpleado });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                throw;
            }
        }

        /// <summary>
        /// Registrar un Empleado.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Empleado
        ///     {
        ///        "codigoEmpleado": 0,
        ///        "nombres": "string",
        ///        "apellidoPaterno": "string",
        ///        "apellidoMaterno": "string",
        ///        "fechaInicio": "2022-11-12T12:38:40.550Z",
        ///        "sucursal": "string",
        ///        "ci": "string",
        ///        "correo": "string",
        ///        "telefono": "string",
        ///        "direccion": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Empleado empleado)
        {
            try
            {
                _db.Empleados.Add(empleado);
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
        /// Registrar un Empleado.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Empleado
        ///     {
        ///        "id" : 0,
        ///        "codigoEmpleado": 0,
        ///        "nombres": "string",
        ///        "apellidoPaterno": "string",
        ///        "apellidoMaterno": "string",
        ///        "fechaInicio": "2022-11-12T12:38:40.550Z",
        ///        "sucursal": "string",
        ///        "ci": "string",
        ///        "correo": "string",
        ///        "telefono": "string",
        ///        "direccion": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [Route("Modificar")]
        public IActionResult Modificar([FromBody] UpdateEmpleadoModel updateEmpleadoModel)
        {
            Empleado oEmpleado = _db.Empleados.Find(updateEmpleadoModel.Id);

            if (oEmpleado == null)
            {
                return BadRequest("Empleado no encontrado");
            }

            try
            {
                //oEmpleado.CodigoEmpleado = updateEmpleadoModel.CodigoEmpleado is null ? oEmpleado.CodigoEmpleado : updateEmpleadoModel.CodigoEmpleado;
                oEmpleado.CodigoEmpleado = updateEmpleadoModel.CodigoEmpleado is null ? oEmpleado.CodigoEmpleado : updateEmpleadoModel.CodigoEmpleado;
                oEmpleado.Nombres = updateEmpleadoModel.Nombres is null ? oEmpleado.Nombres : updateEmpleadoModel.Nombres;
                oEmpleado.ApellidoPaterno = updateEmpleadoModel.ApellidoPaterno is null ? oEmpleado.ApellidoPaterno : updateEmpleadoModel.ApellidoPaterno;
                oEmpleado.ApellidoMaterno = updateEmpleadoModel.ApellidoMaterno is null ? oEmpleado.ApellidoMaterno : updateEmpleadoModel.ApellidoMaterno;
                oEmpleado.FechaInicio = updateEmpleadoModel.FechaInicio is null ? oEmpleado.FechaInicio : updateEmpleadoModel.FechaInicio;
                oEmpleado.Sucursal = updateEmpleadoModel.Sucursal is null ? oEmpleado.Sucursal : updateEmpleadoModel.Sucursal;
                oEmpleado.CI = updateEmpleadoModel.CI is null ? oEmpleado.CI : updateEmpleadoModel.CI;
                oEmpleado.Correo = updateEmpleadoModel.Correo is null ? oEmpleado.Correo : updateEmpleadoModel.Correo;
                oEmpleado.Telefono = updateEmpleadoModel.Telefono is null ? oEmpleado.Telefono : updateEmpleadoModel.Telefono;
                oEmpleado.Direccion = updateEmpleadoModel.Direccion is null ? oEmpleado.Direccion : updateEmpleadoModel.Direccion;
                

                _db.Empleados.Update(oEmpleado);
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
        /// Elimina un empleado en especifico en funcion del Id. 
        /// </summary>
        /// <param name="empleadoId"></param>
        [HttpDelete]
        [Route("Eliminar/{empleadoId:int}")]
        public IActionResult Eliminar(int empleadoId)
        {
            Empleado oEmpleado = _db.Empleados.Find(empleadoId);

            if (oEmpleado == null)
            {
                return BadRequest("Empleado no encontrado");
            }

            try
            {

                _db.Empleados.Remove(oEmpleado);
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
