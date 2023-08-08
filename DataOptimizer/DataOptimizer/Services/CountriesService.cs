using DataOptimizer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DataOptimizer.Services;

public interface ICountriesService
{
    Task<IEnumerable<Country>> GetCountriesAsync(int page = 1, int numberOfItems = 15, string? countryName = null, int? populationInMillions = null, string? sortOrder = null);
}

public class CountriesService : ICountriesService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CountriesService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    public async Task<IEnumerable<Country>> GetCountriesAsync(int page = 1, int numberOfItems = 15, string? countryName = null, int? populationInMillions = null, string? sortOrder = null)
    {
        var url = _configuration["RestCountriesURL"];
        var result = await _httpClient.GetAsync(url);

        var json = await result.Content.ReadFromJsonAsync<IEnumerable<Country>>();

        if(!string.IsNullOrEmpty(countryName))
        {
            json = json?.Where(x => x.Name.Common.ToLower().Contains(countryName.ToLower()));
        }

        if (populationInMillions is not null and > 0)
        {
            json = json?.Where(x => x.Population <= populationInMillions.Value * 1_000_000);
        }

        if (!string.IsNullOrEmpty(sortOrder))
        {
            if (sortOrder is "asc" or "ascend")
            {
                json = json?.OrderBy(x => x.Name.Common.ToString());
            }
            else if (sortOrder is "desc" or "descend")
            {
                json = json?.OrderByDescending(x => x.Name.Common.ToString());
            }
        }

        return json!.Skip((page - 1) * numberOfItems)
            .Take(numberOfItems);
    }
}
