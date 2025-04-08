using System;
using ElectricityServiceApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var providerName = configuration["ProviderName"] ?? "Default Provider";

Console.WriteLine($"Provider Name: {providerName}"); // Debugging: Print to console

var electricityTariffs = new List<ElectricityTariff>
            {
                new ElectricityTariff () { Provider = providerName, Name = "Product A", Type = 1, BaseCost = Random.Shared.Next(5,15), AdditionalKwhCost = Random.Shared.Next(20,30) },
                new ElectricityTariff () { Provider = providerName, Name = "Product B", Type = 2, IncludedKwh = 4000, BaseCost = Random.Shared.Next(800, 900), AdditionalKwhCost = Random.Shared.Next(30,40) }
            };

builder.Services.AddSingleton(electricityTariffs);
builder.Services.AddSingleton(providerName);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
