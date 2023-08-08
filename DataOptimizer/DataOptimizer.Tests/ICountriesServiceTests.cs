using DataOptimizer.Models;
using DataOptimizer.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace DataOptimizer.Tests;

public class ICountriesServiceTests
{
    private readonly IEnumerable<Country> Countries = new List<Country>
    {
        new Country()
        {
            Name = new Name
            {
                Common = "Ukraine"
            },
            Population = 42_000_000
        },
        new Country()
        {
            Name = new Name
            {
                Common = "USA"
            },
            Population = 350_000_000
        },
        new Country()
        {
            Name = new Name
            {
                Common = "Poland"
            },
            Population = 38_000_000
        },
        new Country()
        {
            Name = new Name
            {
                Common = "France"
            },
            Population = 68_000_000
        },
        new Country()
        {
            Name = new Name
            {
                Common = "Germany"
            },
            Population = 83_000_000
        },
    };



    [Fact]
    public async Task GetAllCountries()
    {
        
        var handlerMock = new Mock<HttpMessageHandler>();
        var countriesResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(Countries)),
        };
        handlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync(countriesResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(cfg => cfg["RestCountriesURL"]).Returns("http://example.com");

        var service = new CountriesService(httpClientFactoryMock.Object, configurationMock.Object);

        var res = await service.GetCountriesAsync();

        Assert.Equal(Countries.Count(), res.Count());
    }

    [Fact]
    public async Task GetCountriesFilteredByName()
    {
        var expectedResult = Countries.Single(x => x.Name.Common == "Ukraine");

        var handlerMock = new Mock<HttpMessageHandler>();
        var countriesResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(Countries)),
        };
        handlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync(countriesResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(cfg => cfg["RestCountriesURL"]).Returns("http://example.com");

        var service = new CountriesService(httpClientFactoryMock.Object, configurationMock.Object);

        var res = await service.GetCountriesAsync(countryName: "Ukraine");

        Assert.Equal(expectedResult.Name.Common, res.First().Name.Common);
        Assert.Equal(expectedResult.Population, res.First().Population);
    }

    [Fact]
    public async Task GetCountriesFilteredByPopulation()
    {
        var expectedResult = Countries.Single(x => x.Name.Common == "Poland");

        var handlerMock = new Mock<HttpMessageHandler>();
        var countriesResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(Countries)),
        };
        handlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync(countriesResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(cfg => cfg["RestCountriesURL"]).Returns("http://example.com");

        var service = new CountriesService(httpClientFactoryMock.Object, configurationMock.Object);

        var res = await service.GetCountriesAsync(populationInMillions: 40);

        Assert.Equal(expectedResult.Name.Common, res.First().Name.Common);
        Assert.Equal(expectedResult.Population, res.First().Population);
    }

    [Fact]
    public async Task GetCountriesPaginated()
    {
        var expectedResult = Countries.Skip(2).Take(2).ToArray();

        var handlerMock = new Mock<HttpMessageHandler>();
        var countriesResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(Countries)),
        };
        handlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync(countriesResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(cfg => cfg["RestCountriesURL"]).Returns("http://example.com");

        var service = new CountriesService(httpClientFactoryMock.Object, configurationMock.Object);

        var res = (await service.GetCountriesAsync(page: 2, numberOfItems: 2)).ToArray();

        Assert.Equal(expectedResult[0].Name.Common, res[0].Name.Common);
        Assert.Equal(expectedResult[0].Population, res[0].Population);
        Assert.Equal(expectedResult[1].Name.Common, res[1].Name.Common);
        Assert.Equal(expectedResult[1].Population, res[1].Population);
    }

    [Fact]
    public async Task GetCountriesPaginatedAndSorted()
    {
        var expectedResult = Countries.OrderByDescending(x => x.Name.Common).Skip(2).Take(2).ToArray();

        var handlerMock = new Mock<HttpMessageHandler>();
        var countriesResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(Countries)),
        };
        handlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync(countriesResponse);
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(cfg => cfg["RestCountriesURL"]).Returns("http://example.com");

        var service = new CountriesService(httpClientFactoryMock.Object, configurationMock.Object);

        var res = (await service.GetCountriesAsync(page: 2, numberOfItems: 2, sortOrder: "desc")).ToArray();

        Assert.Equal(expectedResult[0].Name.Common, res[0].Name.Common);
        Assert.Equal(expectedResult[0].Population, res[0].Population);
        Assert.Equal(expectedResult[1].Name.Common, res[1].Name.Common);
        Assert.Equal(expectedResult[1].Population, res[1].Population);
    }
}