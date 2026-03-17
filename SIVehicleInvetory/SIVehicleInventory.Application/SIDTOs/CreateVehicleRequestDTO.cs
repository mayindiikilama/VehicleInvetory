using System.ComponentModel.DataAnnotations;

namespace SIVehicleInventory.Application.SIDTOs
{
    public class CreateVehicleRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string VehicleCode { get; set; } = null!;

        [Required]
        public Guid LocationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string VehicleType { get; set; } = null!;
    }
}
