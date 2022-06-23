using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Validators
{
    public class FileValidator
    {
        private readonly ILogger<FileValidator> _logger;
        private readonly FileOperator _fileOperator;

        public FileValidator(ILogger<FileValidator> logger, FileOperator fileOperator)
        {
            _logger = logger;
            _fileOperator = fileOperator;
        }

        public async Task ValidatePostFileInputParametersAsync(User requestingUser, IFormFile file)
        {
            await Task.CompletedTask;
        }
    }
}
