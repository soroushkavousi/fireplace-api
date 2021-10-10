using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Services
{
    public class FileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly FileValidator _fileValidator;
        private readonly FileOperator _fileOperator;

        public FileService(ILogger<FileService> logger, FileValidator fileValidator, FileOperator fileOperator)
        {
            _logger = logger;
            _fileValidator = fileValidator;
            _fileOperator = fileOperator;
        }

        public async Task<File> CreateFileAsync(User requesterUser, IFormFile formFile)
        {
            await _fileValidator.ValidatePostFileInputParametersAsync(requesterUser, formFile);
            var file = await _fileOperator.CreateFileAsync(formFile);
            return file;
        }
    }
}
