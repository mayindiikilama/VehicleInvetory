using SIVehicleInventory.Domain.SIEnums;

namespace SIVehicleInventory.Application.SIDTOs
{
    public class VehicleResponseDTO
    {
        public Guid Id { get; set; }
        public string VehicleCode { get; set; } = null!;
        public Guid LocationId { get; set; }
        public string VehicleType { get; set; } = null!;
        public SIVehicleStatus Status { get; set; }
    }
}
