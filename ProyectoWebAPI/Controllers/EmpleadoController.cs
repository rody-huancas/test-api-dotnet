using Microsoft.AspNetCore.Mvc;
using ProyectoData;
using ProyectoModelo;

namespace ProyectoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoData _empleadoData;
        public EmpleadoController(EmpleadoData empleadoData)
        {
            _empleadoData = empleadoData;
        }


        [HttpGet]
        public async Task<IActionResult> GetEmpleados()
        {
            List<Empleado> lista = await _empleadoData.ListarEmpleados();

            return StatusCode(StatusCodes.Status200OK, lista);
        }

        
        [HttpPost]
        public async Task<IActionResult> CrearEmpleado([FromBody] Empleado objeto)
        {
            Boolean respuesta = await _empleadoData.RegistrarEmpleado(objeto);

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }


        [HttpPut("id")]
        public async Task<IActionResult> EditarEmpleado([FromBody] Empleado objeto)
        {
            Boolean respuesta = await _empleadoData.EditarEmpleado(objeto);

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEmpleado(int id)
        {
            Boolean respuesta = await _empleadoData.EliminarEmpleado(id);

            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
