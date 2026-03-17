using Microsoft.EntityFrameworkCore;
using SIVehicleInventory.Application.SIInterfaces;
using SIVehicleInventory.Domain.SIEntities;
using SIVehicleInventory.Infrastructure.Data;

namespace SIVehicleInventory.Infrastructure.Repositories
{
    public class SIVehicleRepository : ISIVehicleRepository
    {


        private readonly SIInventoryDbContext _dbContext;

        public SIVehicleRepository(SIInventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SIVehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<SIVehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task AddAsync(SIVehicle vehicle, CancellationToken cancellationToken = default)
        {
            await _dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        }

        public Task DeleteAsync(SIVehicle vehicle, CancellationToken cancellationToken = default)
        {
            _dbContext.Vehicles.Remove(vehicle);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

