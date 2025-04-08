using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin() //for test purpose
              .AllowAnyHeader()
              .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;

builder.Services.AddHttpClient();

var useRedis = Environment.GetEnvironmentVariable("USE_REDIS") == "true";
Console.WriteLine($"Environment.GetEnvironmentVariable(\"USE_REDIS\"): {Environment.GetEnvironmentVariable("USE_REDIS")}");
Console.WriteLine($"Use Redis: {useRedis}");
var conntectionString = configuration["Redis:ConnectionString"] ?? "localhost:6379";
if (useRedis)
{
    Console.WriteLine("Redis is enabled and configured.");
}
else
{
    Console.WriteLine("Redis is disabled. Running without caching.");
}

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var redisEnabled = useRedis;
    return redisEnabled ? ConnectionMultiplexer.Connect(conntectionString) : null;
});
builder.Services.AddSingleton<IDistributedCache, MemoryDistributedCache>(); // Fallback if Redis is disabled
builder.Services.AddSingleton<ElectricityProviderService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.Run();
