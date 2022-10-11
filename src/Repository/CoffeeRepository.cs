namespace CoffeeAPIMinimal.Repository
{
    public class CoffeeRepository : ICoffeeRepository
    {
        public async Task<string> CounterAsync(string key, IDistributedCache cache)
        {
            string i = await cache.GetStringAsync(key);
            i ??= "0";
            int counter = int.Parse(i);
            counter++;
            await cache.SetStringAsync(key, counter.ToString());
            var counterString = await cache.GetStringAsync(key);

            return counterString;
        }

        public async Task<object> GetCoffeeAsync(IDistributedCache cache, HttpContext context)
        {
            //Declaring a unit recordKey to set our get the data
            string recordKey = $"WeatherForecast_{DateTime.Now.ToString("yyyyMMdd_hhmm")}";
            Coffee coffees = await cache.GetRecordAsync<Coffee>(recordKey);

            string ipAddress = GetIpAddress(context);

            string counterString = await CounterAsync(ipAddress, cache);

            if (coffees is not null)
            {
                if (!(DateTime.Today.Day == 1 && DateTime.Today.Month == 4))
                {
                    if (int.Parse(counterString) % 5 == 0)
                    {
                        context.Response.StatusCode = 503;
                        return " ";
                    }

                    return coffees;
                }
                else
                {
                    context.Response.StatusCode = 418;
                    return " ";
                }

            }

            coffees = new Coffee
            {
                Message = "Your piping hot coffee is ready",
                Prepared = DateTimeOffset.Now
            };

            await cache.SetRecordAsync(recordKey, coffees);

            return coffees;
        }

        public string GetIpAddress(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;
            string clientIp = "";
            if (remoteIp != null)
            {
                clientIp = remoteIp.ToString();
            }

            return clientIp;
        }
    }
}

