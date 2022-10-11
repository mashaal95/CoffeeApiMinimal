
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
            string recordKey = $"Coffee_{DateTime.Now.ToString("yyyyMMdd_hhmm")}";
            object coffees = await cache.GetRecordAsync<Coffee>(recordKey);

            string ipAddress = GetIpAddress(context);

            string counterString = await CounterAsync(ipAddress, cache);

            coffees = PreparingCoffee(coffees);

            await cache.SetRecordAsync(recordKey, coffees);

            if (coffees is not null)
                coffees = AprilDateCheck(coffees, counterString, context);


            return coffees;
        }



        private object AprilDateCheck(object coffees, string counterString, HttpContext context)
        {

            if (!(DateTime.Today.Day == 1 && DateTime.Today.Month == 4))
            {
                if (int.Parse(counterString) % 5 == 0)
                {
                    context.Response.StatusCode = 503;
                    return string.Empty;
                }
                else
                {
                    return coffees;
                }

            }
            else
            {
                context.Response.StatusCode = 418;
                return string.Empty;
            }

        }

        private Coffee PreparingCoffee(object coffees)
        {
        
                coffees = new Coffee
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTimeOffset.Now
                };


            return (Coffee)coffees;
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

