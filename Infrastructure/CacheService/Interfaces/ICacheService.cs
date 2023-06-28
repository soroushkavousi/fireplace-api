namespace FireplaceApi.Infrastructure.CacheService;

public interface ICacheService { }
public interface ICacheService<TService> : ICacheService where TService : class { }
