using SIVehicleInventory.Domain.SIExceptions;

namespace SIVehicleInventory.WebAPI.Middleware
{
    public class SIGlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public SIGlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continue request pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Set response type
                context.Response.ContentType = "application/json";

                // Decide status code based on error type
                context.Response.StatusCode = ex switch
                {
                    KeyNotFoundException => 404, // Not found
                    SIDomainException => 400, // Business rule error
                    _ => 500 // Server error
                };

                // Return error message
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.Message
                });
            }
        }
    }
}
