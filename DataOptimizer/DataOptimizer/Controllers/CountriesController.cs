using DataOptimizer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataOptimizer.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesService _countriesService;

    public CountriesController(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries(int page = 1, int numberOfItems = 15, string? countryName = null, int? populationInMillions = null, string? sortOrder = null)
    {
        var res = await _countriesService.GetCountriesAsync(countryName, populationInMillions, sortOrder);

        return Ok(res
            .Skip((page - 1) * numberOfItems)
            .Take(numberOfItems));
    }
}
