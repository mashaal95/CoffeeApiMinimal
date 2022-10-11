namespace CoffeeAPIMinimal.Interfaces
{
    public interface ICoffeeRepository
    {
        Task<object> GetCoffeeAsync(IDistributedCache cache, HttpContext context);

        Task<string> UpdateCount(string ipAddress, IDistributedCache cache);

        string GetIpAddress(HttpContext context);


    }
}
