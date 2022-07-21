using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using TPick.Domain.Aggregates;

namespace TPick.App.Services;

public interface IShopDetailsCrawler
{
    Task<ShopDetails?> GetDetails(string shopUrl, CancellationToken cancellationToken);
}

public class ShopDetailsCrawler : IShopDetailsCrawler
{
    private readonly HttpClient _httpClient;
    public ShopDetailsCrawler(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(configuration["ExternalServices:TPickCrawler"]);
    }

    public async Task<ShopDetails?> GetDetails(string shopUrl, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetFromJsonAsync<ShopDetails>($"api/get-shop-details?shopUrl={shopUrl}", cancellationToken: cancellationToken);
        return result;
    }
}

public class ShopDetails
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Address { get; set; }
    public List<ShopSection> Sections { get; set; } = new();
}