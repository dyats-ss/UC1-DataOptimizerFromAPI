using System.Text.Json;

namespace DataOptimizer.Services;

public interface ICountriesService
{
    Task<IEnumerable<JsonElement>> GetCountriesAsync(string? countryName, int? population, string? sortOrder);
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

    public async Task<IEnumerable<JsonElement>> GetCountriesAsync(string? countryName, int? population, string? sortOrder)
    {
        var result = await _httpClient.GetAsync(_configuration.GetValue<string>("RestCountriesURL"));

        var json = await result.Content.ReadFromJsonAsync<IEnumerable<JsonElement>>();

        if(!string.IsNullOrEmpty(countryName))
        {
            json = json.Where(x => x.GetProperty("name").GetProperty("common").ToString().ToLower().Contains(countryName.ToLower()));
        }

        if (population is not null and > 0)
        {
            json = json.Where(x => Convert.ToInt32(x.GetProperty("population").ToString()) <= population.Value * 1_000_000);
        }

        if (!string.IsNullOrEmpty(sortOrder))
        {
            if (sortOrder is "asc" or "ascend")
            {
                json = json.OrderBy(x => x.GetProperty("name").GetProperty("common").ToString());
            }
            else if (sortOrder is "desc" or "descend")
            {
                json = json.OrderByDescending(x => x.GetProperty("name").GetProperty("common").ToString());
            }
        }

        return json;
    }    
}
