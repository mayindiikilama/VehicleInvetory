using SIVehicleInventory.Domain.SIEnums;
using SIVehicleInventory.Domain.SIExceptions;

namespace SIVehicleInventory.Domain.SIEntities
{
    // This class represents a vehicle in my system
    public class SIVehicle
    {
        // Unique ID auto-generated, no duplicates
        public Guid Id { get; private set; }

        // A  string to identify our vehicle 
        public string VehicleCode { get; private set; }

        //The location where the vehicle is stored or available for rent
        public Guid LocationId { get; private set; }

        // Type of vehicle 
        public string VehicleType { get; private set; }

        // The Current status of our vehicle 
        public SIVehicleStatus Status { get; private set; }




        // Our MAIN constructor used when we are creating a vehicle
        public SIVehicle(string vehicleCode, Guid locationId, string vehicleType)
        {
            // If vehicle code is empty then throw error
            if (string.IsNullOrWhiteSpace(vehicleCode))
                throw new ArgumentException("Vehicle code required!", nameof(vehicleCode));

            // If vehicle type is empty then throw error
            if (string.IsNullOrWhiteSpace(vehicleType))
                throw new ArgumentException("Vehicle type required!", nameof(vehicleType));

            // Generate new unique ID
            Id = Guid.NewGuid();

            // Assign values
            VehicleCode = vehicleCode;
            LocationId = locationId;
            VehicleType = vehicleType;

            // Default status when our vehicle created
            Status = SIVehicleStatus.Available;
        }


        // Changes the status to available
        public void MarkAvailable()

        {

            // Cannot make it available if it's reserved
            if (Status == SIVehicleStatus.Reserved)
                throw new SIDomainException("Cannot mark reserved vehicle as available without release.");

            Status = SIVehicleStatus.Available;
        }


        // Change status to Rented
        public void MarkRented()
        {

            // Already rented gives us an error
            if (Status == SIVehicleStatus.Rented)
                throw new SIDomainException("Vehicle selected is out for rent.");

            // Reserved gives us an error which means cannot rent
            if (Status == SIVehicleStatus.Reserved)
                throw new SIDomainException("Vehicle is already reserved and cannot be rented.");


            // In service also gives us an error that you cannot rent
            if (Status == SIVehicleStatus.Serviced)
                throw new SIDomainException("Vehicle that are under service cannot be rented...");

            Status = SIVehicleStatus.Rented;
        }


        // Change our vehicle status to reserved
        public void MarkReserved()
        {

            // Only vehicles that are available can be reserved
            if (Status != SIVehicleStatus.Available)
                throw new SIDomainException("Only vehicles that are available can be reserved....");

            Status = SIVehicleStatus.Reserved;
        }


        // Change our vehicle status to serviced
        public void MarkServiced()
        {

            // Cannot service a rented vehicle
            if (Status == SIVehicleStatus.Rented)
                throw new SIDomainException("Unfortunately rented vehicles are not allowed to be sent to service...");

            Status = SIVehicleStatus.Serviced;
        }
    }
}

