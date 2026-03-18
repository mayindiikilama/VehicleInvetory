using Microsoft.AspNetCore.Mvc;
using SIVehicleInventory.Application.SIDTOs;
using SIVehicleInventory.Application.SIServices;
using SIVehicleInventory.Domain.SIExceptions;

namespace SIVehicleInventory.WebAPI.Controllers
{

    [ApiController]
    [Route("api/v1/vehicles")]
    public class SIVehicleController : Controller
    {
        private readonly ISIVehicleService _vehicleService;

        public SIVehicleController(ISIVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponseDTO>>> GetAll()
        {
            var result = await _vehicleService.GetAllVehiclesAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VehicleResponseDTO>> GetById(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle is null) return NotFound();
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult<VehicleResponseDTO>> Create([FromBody] CreateVehicleRequestDTO request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var created = await _vehicleService.CreateVehicleAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<VehicleResponseDTO>> UpdateStatus(Guid id, [FromBody] UpdateVehicleStatusRequestDTO request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var updated = await _vehicleService.UpdateVehicleStatusAsync(id, request.NewStatus);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (SIDomainException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
