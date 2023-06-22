using Microsoft.AspNetCore.Http;

namespace FireplaceApi.Presentation.Interfaces;

public interface IOutputHeaderDto
{
    public HeaderDictionary GetHeaderDictionary();
}
