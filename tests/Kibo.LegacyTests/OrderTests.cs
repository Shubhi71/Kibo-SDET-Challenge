using Kibo.TestingFramework;

namespace Kibo.LegacyTests;

public class OrderTests : IDisposable
{
    private readonly KiboApiClient _client;

    public OrderTests()
    {
        _client = new KiboApiClient();
    }

    [Fact]
    public async Task CreateOrder_ReturnsSuccess()
    {
        var order = new OrderBuilder()
            .WithCustomerEmail("john.doe@example.com")
            .WithItems(1)
            .Build();

        var response = await _client.PostAsync<Kibo.TestingFramework.Order>(
            "/v1/orders",
            order);

        Assert.Equal(201, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal("Pending", response.Data.Status);
    }

    [Fact]
    public async Task CreateOrder_WithoutTenantHeader_Returns401()
    {
        var order = new OrderBuilder()
            .WithCustomerEmail("no-tenant@example.com")
            .WithItems(1)
            .Build();

        var response = await _client.PostAsync<Kibo.TestingFramework.Order>(
            "/v1/orders",
            order,
            includeTenant: false);

        Assert.Equal(401, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_AfterCreation_StatusBecomesReadyForFulfillment()
    {
        var order = new OrderBuilder()
            .WithCustomerEmail("status-check@example.com")
            .WithItems(1)
            .Build();

        var createResponse = await _client.PostAsync<Kibo.TestingFramework.Order>(
            "/v1/orders",
            order);

        Assert.Equal(201, createResponse.StatusCode);
        Assert.NotNull(createResponse.Data);

        var orderId = createResponse.Data.Id;

        var readyOrder = await Poller.WaitUntilAsync(
            action: () =>
                _client.GetAsync<Kibo.TestingFramework.Order>(
                    $"/v1/orders/{orderId}"),

            condition: response =>
                response.Data != null &&
                response.Data.Status == "ReadyForFulfillment"
        );

        Assert.NotNull(readyOrder.Data);
        Assert.Equal(
            "ReadyForFulfillment",
            readyOrder.Data.Status);
    }

    [Fact]
    public async Task GetOrder_WithInvalidId_Returns404()
    {
        var invalidId = Guid.NewGuid();

        var response = await _client.GetAsync<Kibo.TestingFramework.Order>(
            $"/v1/orders/{invalidId}");

        Assert.Equal(404, response.StatusCode);
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}