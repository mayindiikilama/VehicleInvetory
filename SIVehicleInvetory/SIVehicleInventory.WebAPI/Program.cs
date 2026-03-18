using Microsoft.EntityFrameworkCore;
using SIVehicleInventory.Application.SIInterfaces;
using SIVehicleInventory.Application.SIServices;
using SIVehicleInventory.Infrastructure.Data;
using SIVehicleInventory.Infrastructure.Repositories;
using SIVehicleInventory.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext
builder.Services.AddDbContext<SIInventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Repositories & Services (only once)
builder.Services.AddScoped<ISIVehicleRepository, SIVehicleRepository>();
builder.Services.AddScoped<ISIVehicleService, SIVehicleService>();

// 3. Controllers + Swagger (only once)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<SIGlobalExceptionMiddleware>();
app.MapControllers();

app.Run();
