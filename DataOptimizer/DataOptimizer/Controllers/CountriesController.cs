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
    public async Task<IActionResult> GetCountries(string? countryName, int? p2, string? sortOrder)
    {
       var res = await _countriesService.GetCountriesAsync(countryName, p2, sortOrder);

        return Ok(res);
    }
}
