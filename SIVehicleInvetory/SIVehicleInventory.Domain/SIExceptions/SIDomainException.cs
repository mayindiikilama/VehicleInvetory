namespace SIVehicleInventory.Domain.SIExceptions
{

    // Custom exception (my own error type)
    // Used when our business rules are broken
    public sealed class SIDomainException : Exception
    {

        // Constructor that accepts an error message
        public SIDomainException(string message)
            : base(message) { } // send message to base Exception class
    }
}
