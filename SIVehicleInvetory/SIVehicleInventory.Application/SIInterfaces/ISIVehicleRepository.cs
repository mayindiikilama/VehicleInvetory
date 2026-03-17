using SIVehicleInventory.Domain.SIEntities;

namespace SIVehicleInventory.Application.SIInterfaces
{
    public interface ISIVehicleRepository
    {
        Task<SIVehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SIVehicle>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(SIVehicle vehicle, CancellationToken cancellationToken = default);
        Task DeleteAsync(SIVehicle vehicle, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
