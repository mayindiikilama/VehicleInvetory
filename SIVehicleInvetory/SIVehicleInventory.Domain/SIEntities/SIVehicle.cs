using SIVehicleInventory.Domain.SIEnums;
using SIVehicleInventory.Domain.SIExceptions;

namespace SIVehicleInventory.Domain.SIEntities
{
    public class SIVehicle
    {
        public Guid Id { get; private set; }
        public string VehicleCode { get; private set; }
        public Guid LocationId { get; private set; }
        public string VehicleType { get; private set; }
        public SIVehicleStatus Status { get; private set; }


        private SIVehicle() { }

        public SIVehicle(string vehicleCode, Guid locationId, string vehicleType)
        {
            if (string.IsNullOrWhiteSpace(vehicleCode))
                throw new ArgumentException("Vehicle code is required.", nameof(vehicleCode));

            if (string.IsNullOrWhiteSpace(vehicleType))
                throw new ArgumentException("Vehicle type is required.", nameof(vehicleType));

            Id = Guid.NewGuid();
            VehicleCode = vehicleCode;
            LocationId = locationId;
            VehicleType = vehicleType;
            Status = SIVehicleStatus.Available;
        }

        public void MarkAvailable()
        {
            if (Status == SIVehicleStatus.Reserved)
                throw new SIDomainException("Cannot mark reserved vehicle as available without release.");

            Status = SIVehicleStatus.Available;
        }

        public void MarkRented()
        {
            if (Status == SIVehicleStatus.Rented)
                throw new SIDomainException("Vehicle is already rented.");

            if (Status == SIVehicleStatus.Reserved)
                throw new SIDomainException("Vehicle is reserved and cannot be rented directly.");

            if (Status == SIVehicleStatus.Serviced)
                throw new SIDomainException("Vehicle under service cannot be rented.");

            Status = SIVehicleStatus.Rented;
        }

        public void MarkReserved()
        {
            if (Status != SIVehicleStatus.Available)
                throw new SIDomainException("Only available vehicles can be reserved.");

            Status = SIVehicleStatus.Reserved;
        }

        public void MarkServiced()
        {
            if (Status == SIVehicleStatus.Rented)
                throw new SIDomainException("Rented vehicles cannot be sent to service.");

            Status = SIVehicleStatus.Serviced;
        }
    }
}

