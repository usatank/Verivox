using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CalculationService.Models;
using CalculationService.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace CalculationService.Services
{
    public class ElectricityProviderService : IElectricityProviderService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<ElectricityProviderService> _logger;
        private readonly List<string> _providers;
        private readonly int _cacheTTL;
        private readonly bool _useRedis;

        public ElectricityProviderService(HttpClient httpClient, IConfiguration config, IDistributedCache cache, ILogger<ElectricityProviderService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
            _providers = config.GetSection("ElectricityProviders").Get<List<string>>() ?? new List<string>();
            _cacheTTL = config.GetValue<int>("Redis:CacheTTL");
            _useRedis = Environment.GetEnvironmentVariable("USE_REDIS") == "true";
        }

        public async Task<List<ElectricityTariff>> GetElectricityTariffsAsync()
        {
            var allTariffs = new List<ElectricityTariff>();

            foreach (var providerUrl in _providers)
            {
                var cacheKey = $"tariffs:{providerUrl}";

                // Try to get cached data
                var cachedData = _useRedis ? await _cache.GetStringAsync(cacheKey) : null;
                if (cachedData != null)
                {
                    _logger.LogInformation("Cache hit for {ProviderUrl}", providerUrl);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true //Ignores case mismatches
                    };
                    var cachedTariffs = JsonSerializer.Deserialize<List<ElectricityTariff>>(cachedData, options);
                    if (cachedTariffs != null) allTariffs.AddRange(cachedTariffs);
                    continue;
                }

                // Fetch from API if cache is empty
                try
                {
                    _logger.LogInformation("Fetching electricity tariffs from {ProviderUrl}", providerUrl);
                    var response = await _httpClient.GetAsync($"{providerUrl}/api/electricity");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true //Ignores case mismatches
                    };
                    var tariffs = JsonSerializer.Deserialize<List<ElectricityTariff>>(content, options);
                    if (tariffs != null)
                    {
                        allTariffs.AddRange(tariffs);

                        // Store in cache
                        if (_useRedis)
                        {
                            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(tariffs), new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_cacheTTL)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching electricity tariffs from {ProviderUrl}", providerUrl);
                }
            }

            return allTariffs;
        }
    }
}