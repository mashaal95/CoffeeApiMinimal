

namespace CoffeeAPIMinimal.Tests;

public class UnitTest1
{
    [Fact]
    public void Check_IfRedisConnection_IsAliveAsync()
    {

        var multiplexer = ConnectionMultiplexer.Connect("localhost");
        var response = multiplexer.IsConnected;

        Assert.True(response);
        //


    }


    [Fact]
    public async Task GetCoffee_WhenCalled_ReturnsOkResult()
    {
        await using var application = new WebApplicationFactory<CoffeeRepository>();
        using var client = application.CreateClient();
        var response = await client.GetAsync("/brew-coffee");
        var data = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


}