using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Application.Interfaces;

public interface IOutputHeaderDto
{
    public HeaderDictionary GetHeaderDictionary();
}
