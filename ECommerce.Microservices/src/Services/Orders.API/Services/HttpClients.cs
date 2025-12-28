namespace Orders.API.Services;

public class CatalogHttpClient
{
    private readonly HttpClient _httpClient;

    public CatalogHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProductDto?> GetProductAsync(int productId)
    {
        var response = await _httpClient.GetAsync($"/api/products/{productId}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ProductDto>();
    }
}

public class CustomersHttpClient
{
    private readonly HttpClient _httpClient;

    public CustomersHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CustomerExistsAsync(int customerId)
    {
        var response = await _httpClient.GetAsync($"/api/customers/{customerId}/exists");
        if (!response.IsSuccessStatusCode)
            return false;

        return await response.Content.ReadFromJsonAsync<bool>();
    }
}

public record ProductDto(int Id, string Name, string Description, decimal Price, int StockQuantity);
