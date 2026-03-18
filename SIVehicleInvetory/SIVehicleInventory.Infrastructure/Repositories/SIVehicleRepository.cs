using Microsoft.EntityFrameworkCore;
using SIVehicleInventory.Application.SIInterfaces;
using SIVehicleInventory.Domain.SIEntities;
using SIVehicleInventory.Infrastructure.Data;

namespace SIVehicleInventory.Infrastructure.Repositories
{

    // This class talks directly to the database
    public class SIVehicleRepository : ISIVehicleRepository
    {


        private readonly SIInventoryDbContext _dbContext;


        public SIVehicleRepository(SIInventoryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Get all vehicles by ID
        public async Task<SIVehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles.FindAsync(new object[] { id }, cancellationToken);
        }

        // Get all vehicles
        public async Task<IReadOnlyList<SIVehicle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles.AsNoTracking().ToListAsync(cancellationToken);
        }

        // Add new vehicle
        public async Task AddAsync(SIVehicle vehicle, CancellationToken cancellationToken = default)
        {
            await _dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
        }


        // Delete vehicle
        public Task DeleteAsync(SIVehicle vehicle, CancellationToken cancellationToken = default)
        {
            _dbContext.Vehicles.Remove(vehicle);
            return Task.CompletedTask;
        }

        // Saves all changes to the database
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

