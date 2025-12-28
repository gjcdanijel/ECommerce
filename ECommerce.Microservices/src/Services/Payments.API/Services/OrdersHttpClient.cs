namespace Payments.API.Services;

public class OrdersHttpClient
{
    private readonly HttpClient _httpClient;

    public OrdersHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OrderDto?> GetOrderAsync(int orderId)
    {
        var response = await _httpClient.GetAsync($"/api/orders/{orderId}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<OrderDto>();
    }
}

public record OrderDto(int Id, int CustomerId, int ProductId, int Quantity, decimal TotalAmount, int Status, DateTime CreatedAt);
