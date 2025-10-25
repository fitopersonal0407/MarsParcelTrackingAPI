using MarsParcelTracking.API;
using MarsParcelTracking.Application;
using MarsParcelTracking.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ParcelContext>(opt => opt.UseInMemoryDatabase("ParcelList"));

builder.Services.AddScoped<IParcelService, ParcelService>();
builder.Services.AddScoped<IParcelDataAccess, ParcelDataAccessEF>();

var app = builder.Build();

app.UseMiddleware<MarsParcelAPIMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

public partial class Program { }