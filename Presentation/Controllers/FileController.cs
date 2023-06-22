using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Tools;
using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Services;
using FireplaceApi.Application.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/files")]
[Produces("application/json")]
public class FileController : ApiController
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    /// <summary>
    /// Create a single file.
    /// </summary>
    /// <returns>Created file</returns>
    /// <response code="200">Returns the newly created item</response>
    [Authorize(Policy = AuthConstants.UserPolicyKey, Roles = nameof(UserRole.USER))]
    [ProducesResponseType(typeof(FileDto), StatusCodes.Status200OK)]
    [RequestFormLimits(MultipartBodyLengthLimit = 268435456)]
    [RequestSizeLimit(268435456)] // For kestrel
    [HttpPost]
    public async Task<ActionResult<FileDto>> PostFileAsync(
        [BindNever][FromHeader] RequestingUser requestingUser,
        [FromForm] PostFileInputFormDto inputBodyDto)
    {
        var file = await _fileService.CreateFileAsync(requestingUser.Id.Value, inputBodyDto.FormFile);
        var fileDto = file.ToDto();
        return fileDto;
    }
}
