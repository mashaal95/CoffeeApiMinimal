namespace CoffeeAPIMinimal.Extensions
{
    internal static class DistribuitedCacheExtentions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string recordId,
        T data,
        TimeSpan? absoluteExpireTime = null)
        {
            //This method will be responsible to set the data into the Redis database

            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);

            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache,
        string recordId)
        {
            //This method will be responsible to get the data from the Redis database

            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null) return default;

            var dataResponse = JsonSerializer.Deserialize<T>(jsonData);

            return dataResponse;
        }



    }
}
