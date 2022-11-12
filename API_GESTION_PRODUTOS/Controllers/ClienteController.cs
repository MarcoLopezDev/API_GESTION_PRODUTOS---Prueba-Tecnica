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
    public class ClienteController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ClienteController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Listar todos los Clientes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Listar")]
        public ActionResult Listar()
        {
            List<Cliente> listaClientes = new List<Cliente>();

            try
            {
                listaClientes = _db.Clientes.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = listaClientes });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, reponse = listaClientes });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un cliente en especifico en funcion del Id.
        /// </summary>
        /// <param name="clienteId"></param>
        [HttpGet]
        [Route("Buscar/{clienteId:int}")]
        public ActionResult Buscar(int clienteId)
        {
            Cliente oCliente = _db.Clientes.Find(clienteId);

            if (oCliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }

            try
            {
                oCliente = _db.Clientes.Where(p => p.Id == clienteId).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oCliente });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message, response = oCliente });
                throw;
            }
        }

        /// <summary>
        /// Deveulve un producto en especifico en funcion del CI.
        /// </summary>
        /// <param name="ci"></param>
        [HttpGet]
        [Route("BuscarPorCI/{ci}")]
        public ActionResult BuscarPorCodigo(string ci)
        {
            try
            {
                Cliente oCliente = _db.Clientes.Where(p => p.CI == ci).FirstOrDefault();

                if (oCliente == null)
                {
                    return BadRequest("Cliente no encontrado");
                }

                return StatusCode(StatusCodes.Status200OK, new { message = "OK", response = oCliente });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
                throw;
            }
        }

        /// <summary>
        /// Registrar un Cliente.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Cliente
        ///     {
        ///        "nombres": "string",
        ///        "apellidoPaterno": "string",
        ///        "apellidoMaterno": "string",
        ///        "ci": "string",
        ///        "correo": "string",
        ///        "telefono": "string",
        ///        "direccion": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [Route("Registrar")]
        public IActionResult Registrar([FromBody] Cliente cliente)
        {
            try
                {
                _db.Clientes.Add(cliente);
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
        /// Modificar un Cliente.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Registrar - Cliente
        ///     {
        ///        "id" : 0,
        ///        "nombres": "string",
        ///        "apellidoPaterno": "string",
        ///        "apellidoMaterno": "string",
        ///        "ci": "string",
        ///        "correo": "string",
        ///        "telefono": "string",
        ///        "direccion": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        [Route("Modificar")]
        public IActionResult Modificar([FromBody] UpdateClienteModel updateClienteModel)
        {
            Cliente oCliente = _db.Clientes.Find(updateClienteModel.Id);

            if (oCliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }

            try
            {
                oCliente.Nombres = updateClienteModel.Nombres is null ? oCliente.Nombres : updateClienteModel.Nombres;
                oCliente.ApellidoPaterno = updateClienteModel.ApellidoPaterno is null ? oCliente.ApellidoPaterno : updateClienteModel.ApellidoPaterno;
                oCliente.ApellidoMaterno = updateClienteModel.ApellidoMaterno is null ? oCliente.ApellidoMaterno : updateClienteModel.ApellidoMaterno;
                oCliente.CI = updateClienteModel.CI is null ? oCliente.CI : updateClienteModel.CI;
                oCliente.Correo = updateClienteModel.Correo is null ? oCliente.Correo : updateClienteModel.Correo;
                oCliente.Telefono = updateClienteModel.Telefono is null ? oCliente.Telefono : updateClienteModel.Telefono;
                oCliente.Direccion = updateClienteModel.Direccion is null ? oCliente.Direccion : updateClienteModel.Direccion;

                _db.Clientes.Update(oCliente);
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
        /// Elimina un cliente en especifico en funcion del Id. 
        /// </summary>
        /// <param name="clienteId"></param>
        [HttpDelete]
        [Route("Eliminar/{clienteId:int}")]
        public IActionResult Eliminar(int clienteId)
        {
            Cliente oCliente = _db.Clientes.Find(clienteId);

            if (oCliente == null)
            {
                return BadRequest("Cliente no encontrado");
            }

            try
            {

                _db.Clientes.Remove(oCliente);
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
