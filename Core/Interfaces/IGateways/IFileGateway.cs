using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IFileGateway
    {
        public Task CreateFileAsync(IFormFile formFile, string filePhysicalPath);
    }
}
