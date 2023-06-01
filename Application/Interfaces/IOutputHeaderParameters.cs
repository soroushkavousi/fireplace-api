using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Application.Interfaces;

public interface IOutputHeaderParameters
{
    public HeaderDictionary GetHeaderDictionary();
}
