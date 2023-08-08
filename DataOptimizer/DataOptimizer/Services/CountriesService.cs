using System.Text.Json;

namespace DataOptimizer.Services;

public interface ICountriesService
{
    Task<List<JsonElement>> GetCountriesAsync(string? countryName, int? population, string? p3);
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

    public async Task<List<JsonElement>> GetCountriesAsync(string? countryName, int? population, string? p3)
    {
        var result = await _httpClient.GetAsync(_configuration.GetValue<string>("RestCountriesURL"));

        var json = await result.Content.ReadFromJsonAsync<List<JsonElement>>();

        if(!string.IsNullOrEmpty(countryName))
        {
            json = json.Where(x => x.GetProperty("name").GetProperty("common").ToString().ToLower().Contains(countryName.ToLower())).ToList();
        }

        if (population is not null and > 0)
        {
            json = json.Where(x => Convert.ToInt32(x.GetProperty("population").ToString()) <= population.Value * 1_000_000).ToList();
        }

        return json;
    }    
}
