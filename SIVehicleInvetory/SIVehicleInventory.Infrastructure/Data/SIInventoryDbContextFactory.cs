using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SIVehicleInventory.Infrastructure.Data
{
    public class SIInventoryDbContextFactory : IDesignTimeDbContextFactory<SIInventoryDbContext>
    {
        public SIInventoryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SIInventoryDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SIVehicleInventoryDb;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new SIInventoryDbContext(optionsBuilder.Options);
        }
    }
}
