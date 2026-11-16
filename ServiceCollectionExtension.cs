using StackExchange.Redis;

namespace RateLimitLab;

public static class ServiceCollectionExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRedisRateLimit()
        {
            services.AddSingleton(sp =>
            {
                var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
                return new TokenBucket(
                    connectionMultiplexer,
                    capacity: 10,
                    refillRate: 1,
                    refillIntervalSeconds: 30
                );
            });
            return services;
        }
    }
}
