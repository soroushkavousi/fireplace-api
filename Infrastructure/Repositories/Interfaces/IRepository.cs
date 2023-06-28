namespace FireplaceApi.Infrastructure.Repositories;

public interface IRepository { }
public interface IRepository<TService> : IRepository where TService : class { }
