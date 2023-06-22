using System;

namespace FireplaceApi.Presentation.Interfaces;

public interface IValidator
{
    public void Validate(IServiceProvider serviceProvider);
}
