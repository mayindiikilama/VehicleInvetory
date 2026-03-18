namespace SIVehicleInventory.Domain.SIEnums
{

    // Instead of typing strings like "Available", we use this (safer)
    public enum SIVehicleStatus
    {
        // Vehicle is free for us to use
        Available = 0,

        // Vehicle selected is rented out at the time of the request
        Rented = 1,

        // Vehicle has been booked but it is not yet picked up
        Reserved = 2,

        // Vehicle is in maintenance
        Serviced = 3
    }
}

