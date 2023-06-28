using FireplaceApi.Domain.Files;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Files;

public class FileService
{
    private readonly IServerLogger<FileService> _logger;
    private readonly FileValidator _fileValidator;
    private readonly FileOperator _fileOperator;

    public FileService(IServerLogger<FileService> logger, FileValidator fileValidator, FileOperator fileOperator)
    {
        _logger = logger;
        _fileValidator = fileValidator;
        _fileOperator = fileOperator;
    }

    public async Task<File> CreateFileAsync(ulong userId, IFormFile formFile)
    {
        await _fileValidator.ValidatePostFileInputParametersAsync(userId, formFile);
        var file = await _fileOperator.CreateFileAsync(formFile);
        return file;
    }
}
