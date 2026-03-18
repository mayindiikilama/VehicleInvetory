using SIVehicleInventory.Application.SIDTOs;
using SIVehicleInventory.Application.SIInterfaces;
using SIVehicleInventory.Domain.SIEntities;
using SIVehicleInventory.Domain.SIEnums;

namespace SIVehicleInventory.Application.SIServices
{
    public class SIVehicleService : ISIVehicleService
    {
        private readonly ISIVehicleRepository _repository;

        //public SIVehicleService(ISIVehicleRepository repository)
        //{
        //    _repository = repository;
        //}

        public async Task<VehicleResponseDTO> CreateVehicleAsync(CreateVehicleRequestDTO request)
        {
            var vehicle = new SIVehicle(request.VehicleCode, request.LocationId, request.VehicleType);

            await _repository.AddAsync(vehicle);
            await _repository.SaveChangesAsync();

            return ToDto(vehicle);
        }

        public async Task<VehicleResponseDTO?> GetVehicleByIdAsync(Guid id)
        {
            var vehicle = await _repository.GetByIdAsync(id);
            return vehicle is null ? null : ToDto(vehicle);
        }

        public async Task<IReadOnlyList<VehicleResponseDTO>> GetAllVehiclesAsync()
        {
            var vehicles = await _repository.GetAllAsync();
            return vehicles.Select(ToDto).ToList();
        }

        public async Task<VehicleResponseDTO> UpdateVehicleStatusAsync(Guid id, SIVehicleStatus newStatus)
        {
            var vehicle = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Sorry, the vehicle is not found.");

            switch (newStatus)
            {
                case SIVehicleStatus.Available:
                    vehicle.MarkAvailable();
                    break;
                case SIVehicleStatus.Reserved:
                    vehicle.MarkReserved();
                    break;
                case SIVehicleStatus.Rented:
                    vehicle.MarkRented();
                    break;
                case SIVehicleStatus.Serviced:
                    vehicle.MarkServiced();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus));
            }

            await _repository.SaveChangesAsync();
            return ToDto(vehicle);
        }

        public async Task DeleteVehicleAsync(Guid id)
        {
            var vehicle = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException(" Sorry, the vehicle is not found.");

            await _repository.DeleteAsync(vehicle);
            await _repository.SaveChangesAsync();
        }

        private static VehicleResponseDTO ToDto(SIVehicle v) =>
            new()
            {
                Id = v.Id,
                VehicleCode = v.VehicleCode,
                LocationId = v.LocationId,
                VehicleType = v.VehicleType,
                Status = v.Status
            };
    }
}

