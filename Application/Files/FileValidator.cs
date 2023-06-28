using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Files;

public class FileValidator
{
    private readonly IServerLogger<FileValidator> _logger;
    private readonly FileOperator _fileOperator;

    public FileValidator(IServerLogger<FileValidator> logger, FileOperator fileOperator)
    {
        _logger = logger;
        _fileOperator = fileOperator;
    }

    public async Task ValidatePostFileInputParametersAsync(ulong userId, IFormFile file)
    {
        await Task.CompletedTask;
    }
}
