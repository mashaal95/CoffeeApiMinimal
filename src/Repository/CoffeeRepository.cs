using System.Globalization;

namespace CoffeeAPIMinimal.Repository
{
    public class CoffeeRepository : ICoffeeRepository
    {
       
        public async Task<string> UpdateCount(string key, IDistributedCache cache)
        {
            int currentCount = await GetCount(key, cache);
            currentCount++;
            await SetCount(key, cache, currentCount);

            return currentCount.ToString();
        }

        public async Task<int> GetCount(string key, IDistributedCache cache)
        {
            string response = await cache.GetStringAsync(key);
            response ??= "0";
            int currentCount = int.Parse(response);

            return currentCount;
        }

        public async Task SetCount(string key, IDistributedCache cache, int newCount)
        {
            await cache.SetStringAsync(key, newCount.ToString());

        }

        public async Task<object> GetCoffeeAsync(IDistributedCache cache, HttpContext context)
        {

            // Using the IP address to uniquely identify the user of the API
            string ipAddress = GetIpAddress(context);

            string currentCount = await UpdateCount(ipAddress, cache);


            object coffee = PreparingCoffee();


            if (coffee is not null)
                coffee = AmITeapot(coffee, currentCount, context, DateTime.Now);

            return coffee;
        }


        private object AmITeapot(object coffees, string counterString, HttpContext context, DateTime dateTime)
        {

            if (!(dateTime.Day == 1 && dateTime.Month == 4))
            {

                return CheckService(coffees, counterString, context);
            }
            else
            {
                context.Response.StatusCode = 418;
                return string.Empty;
            }

        }

        private object CheckService(object coffees, string counterString, HttpContext context)
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


        private Coffee PreparingCoffee()
        {
     
                var coffees = new Coffee
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:sszzz", CultureInfo.InvariantCulture)
                };


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

