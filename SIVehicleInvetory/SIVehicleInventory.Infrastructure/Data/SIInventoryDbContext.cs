using Microsoft.EntityFrameworkCore;
using SIVehicleInventory.Domain.SIEntities;

namespace SIVehicleInventory.Infrastructure.Data
{
    public class SIInventoryDbContext : DbContext
    {
        public SIInventoryDbContext(DbContextOptions<SIInventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<SIVehicle> Vehicles => Set<SIVehicle>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SIVehicle>(builder =>
            {
                builder.HasKey(v => v.Id);

                builder.Property(v => v.VehicleCode)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(v => v.VehicleType)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(v => v.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });
        }
    }
}

