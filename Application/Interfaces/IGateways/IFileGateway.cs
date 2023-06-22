using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Interfaces;

public interface IFileGateway
{
    public Task CreateFileAsync(IFormFile formFile, string filePhysicalPath);
}
