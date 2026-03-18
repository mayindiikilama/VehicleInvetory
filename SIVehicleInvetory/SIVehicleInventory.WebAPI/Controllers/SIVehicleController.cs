using Microsoft.AspNetCore.Mvc;
using SIVehicleInventory.Application.SIDTOs;
using SIVehicleInventory.Application.SIServices;

namespace SIVehicleInventory.WebAPI.Controllers
{

    // This handles HTTP requests
    [ApiController]
    // Base URL for all endpoints in this controller
    [Route("api/v1/vehicles")]
    public class SIVehicleController : Controller
    {
        // This connects controller to service layer
        private readonly ISIVehicleService _vehicleService;

        // Constructor (Dependency Injection)
        // ASP.NET automatically gives us the service
        public SIVehicleController(ISIVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET api/vehicle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleResponseDTO>>> GetAll()
        {
            var result = await _vehicleService.GetAllVehiclesAsync();
            return Ok(result);
        }

        // GET api/v1/vehicles/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VehicleResponseDTO>> GetById(Guid id)
        {
            // Ask service for vehicle
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);

            if (vehicle is null) return NotFound();
            return Ok(vehicle);
        }

        // POST api/vehicle
        [HttpPost]
        public async Task<ActionResult<VehicleResponseDTO>> Create([FromBody] CreateVehicleRequestDTO request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var created = await _vehicleService.CreateVehicleAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/v1/vehicles/{id}/status
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<VehicleResponseDTO>> UpdateStatus(Guid id, [FromBody] UpdateVehicleStatusRequestDTO request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            //try
            //{
            //    // Call service to update status
            //    var updated = await _vehicleService.UpdateVehicleStatusAsync(id, request.NewStatus);
            //    return Ok(updated);
            //}
            //catch (KeyNotFoundException)
            //{
            //    return NotFound();
            //}
            //catch (SIDomainException ex)
            //{
            //    // If business rule is broken it gives us error 404
            //    return BadRequest(new { error = ex.Message });
            //}
            var updated = await _vehicleService.UpdateVehicleStatusAsync(id, request.NewStatus);

            return Ok(updated);
        }

        // DELETE api/v1/vehicles/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //try
            //{
            //    // Ask service to delete vehicle
            //    await _vehicleService.DeleteVehicleAsync(id);
            //    return NoContent();
            //}
            //catch (KeyNotFoundException)
            //{
            //    return NotFound();
            //}
            await _vehicleService.DeleteVehicleAsync(id);

            return NoContent();
        }
    }
}
