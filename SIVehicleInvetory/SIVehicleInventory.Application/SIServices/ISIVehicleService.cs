using SIVehicleInventory.Application.SIDTOs;
using SIVehicleInventory.Domain.SIEnums;

namespace SIVehicleInventory.Application.SIServices
{
    public interface ISIVehicleService
    {
        Task<VehicleResponseDTO> CreateVehicleAsync(CreateVehicleRequestDTO request);
        Task<VehicleResponseDTO?> GetVehicleByIdAsync(Guid id);
        Task<IReadOnlyList<VehicleResponseDTO>> GetAllVehiclesAsync();
        Task<VehicleResponseDTO> UpdateVehicleStatusAsync(Guid id, SIVehicleStatus newStatus);
        Task DeleteVehicleAsync(Guid id);
    }
}

