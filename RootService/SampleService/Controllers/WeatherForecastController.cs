using Microsoft.AspNetCore.Mvc;
using SampleService.Services.Clients;

namespace SampleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        //private readonly IHttpClientFactory _httpClientFactory;
        //private readonly HttpClient _httpClient;
        private IRootServiceClient _rootServiceClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            /*IHttpClientFactory httpClientFactory*/
            IRootServiceClient rootServiceClient)
        {
            _logger = logger;
            _rootServiceClient = rootServiceClient;
            //_httpClientFactory = httpClientFactory;
            //_httpClient = _httpClientFactory.CreateClient("RootServiceClient");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<RootServiceNamespace.WeatherForecast>>> Get()
        {
            _logger.LogInformation("WeatherForecastController >>> START  GetWeatherForecast");
            //RootServiceNamespace.RootServiceClient rootServiceClient =
            //    new RootServiceNamespace.RootServiceClient("http://localhost:5205/", _httpClient);
            var res = await _rootServiceClient.Get();
            _logger.LogInformation("WeatherForecastController >>> END  GetWeatherForecast");
            //return Ok(await rootServiceClient.GetWeatherForecastAsync());            
            return Ok(res);
        }
    }
}