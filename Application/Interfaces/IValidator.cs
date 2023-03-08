using System;

namespace FireplaceApi.Application.Interfaces
{
    public interface IValidator
    {
        public void Validate(IServiceProvider serviceProvider);
    }
}
