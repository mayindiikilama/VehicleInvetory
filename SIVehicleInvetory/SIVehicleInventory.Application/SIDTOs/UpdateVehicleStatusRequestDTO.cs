using SIVehicleInventory.Domain.SIEnums;
using System.ComponentModel.DataAnnotations;

namespace SIVehicleInventory.Application.SIDTOs
{
    public class UpdateVehicleStatusRequestDTO
    {
        [Required]
        public SIVehicleStatus NewStatus { get; set; }
    }
}
