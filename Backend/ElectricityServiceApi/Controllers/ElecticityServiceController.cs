using Microsoft.AspNetCore.Mvc;
using ElectricityServiceApi.Models;

namespace ElectricityServiceApi.Controllers
{
    [ApiController]
    [Route("api/electricity")]
    public class ElecticityServiceController : ControllerBase
    {
        private readonly string _providerName;
        private readonly ILogger<ElecticityServiceController> _logger;
        private readonly List<ElectricityTariff> _electricityTariffs;

        public ElecticityServiceController(string providerName, List<ElectricityTariff> electricityTariffs, ILogger<ElecticityServiceController> logger)
        {
            _providerName = providerName;
            _logger = logger;
            _electricityTariffs = electricityTariffs;
            
        }
                
        [HttpGet(Name = "Get")]
        public IResult Get()
        {            
            return Results.Json(_electricityTariffs);
        }
    }
}
