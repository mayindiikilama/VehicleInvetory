
using SIVehicleInventory.ApiGateway;

var builder = WebApplication.CreateBuilder(args);




// Add API Key auth
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<SIApiKeyAuthenticationOptions, SIApiKeyAuthenticationHandler>(
        "ApiKey", options => { });
builder.Services.AddAuthorization();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


app.MapReverseProxy().RequireAuthorization();

app.Run();
