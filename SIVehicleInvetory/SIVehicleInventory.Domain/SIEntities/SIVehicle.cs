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
                throw new ArgumentException("Vehicle code required!", nameof(vehicleCode));

            if (string.IsNullOrWhiteSpace(vehicleType))
                throw new ArgumentException("Vehicle type required!", nameof(vehicleType));

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
                throw new SIDomainException("Vehicle selected is out for rent.");

            if (Status == SIVehicleStatus.Reserved)
                throw new SIDomainException("Vehicle is already reserved and cannot be rented.");

            if (Status == SIVehicleStatus.Serviced)
                throw new SIDomainException("Vehicle that are under service cannot be rented...");

            Status = SIVehicleStatus.Rented;
        }

        public void MarkReserved()
        {
            if (Status != SIVehicleStatus.Available)
                throw new SIDomainException("Only vehicles that are available can be reserved....");

            Status = SIVehicleStatus.Reserved;
        }

        public void MarkServiced()
        {
            if (Status == SIVehicleStatus.Rented)
                throw new SIDomainException("Unfortunately rented vehicles are not allowed to be sent to service...");

            Status = SIVehicleStatus.Serviced;
        }
    }
}

