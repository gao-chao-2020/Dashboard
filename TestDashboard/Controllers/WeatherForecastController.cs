using Microsoft.AspNetCore.Mvc;

namespace TestDashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            _logger.LogInformation("«Î«Û≥…π¶");
            return await Task.FromResult("Hello World!");
        }
    }
}
