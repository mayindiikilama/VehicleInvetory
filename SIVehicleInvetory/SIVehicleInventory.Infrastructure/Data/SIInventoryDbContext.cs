using Microsoft.EntityFrameworkCore;
using SIVehicleInventory.Domain.SIEntities;

namespace SIVehicleInventory.Infrastructure.Data
{

    // This is the class that connects my app to SQL Server
    public class SIInventoryDbContext : DbContext
    {

        //My constructor receives configuration from my Program.cs
        public SIInventoryDbContext(DbContextOptions<SIInventoryDbContext> options)
            : base(options)
        {
        }


        // This represents my Vehicles table in my database
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

