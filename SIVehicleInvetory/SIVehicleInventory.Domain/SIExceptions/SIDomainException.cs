namespace SIVehicleInventory.Domain.SIExceptions
{
    public sealed class SIDomainException : Exception
    {
        public SIDomainException(string message) : base(message) { }
    }
}
