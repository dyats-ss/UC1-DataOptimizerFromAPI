using Microsoft.AspNetCore.Mvc;

namespace DataOptimizer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public CountriesController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries(string? p1, int? p2, string? p3)
    {
        var result = await _httpClient.GetAsync("https://restcountries.com/v3.1/all");

        return Ok(result.Content.ReadFromJsonAsync<object>());
    }
}
