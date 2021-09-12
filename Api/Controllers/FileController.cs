using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Converters;
using FireplaceApi.Api.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using FireplaceApi.Core.Services;
using FireplaceApi.Core.Models;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/files")]
    [Produces("application/json")]
    public class FileController : ApiController
    {
        private readonly FileConverter _fileConverter;
        private readonly FileService _fileService;

        public FileController(FileConverter fileConverter, FileService fileService)
        {
            _fileConverter = fileConverter;
            _fileService = fileService;
        }

        /// <summary>
        /// Create a single file.
        /// </summary>
        /// <returns>Created file</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost]
        [ProducesResponseType(typeof(FileDto), StatusCodes.Status200OK)]
        [RequestFormLimits(MultipartBodyLengthLimit = 268435456)]
        [RequestSizeLimit(268435456)] // For kestrel
        public async Task<ActionResult<FileDto>> PostFileAsync(
            [BindNever] [FromHeader] User requesterUser,
            [FromForm] ControllerPostFileInputFormParameters inputBodyParameters)
        {
            var file = await _fileService.CreateFileAsync(requesterUser, inputBodyParameters.FormFile);
            var fileDto = _fileConverter.ConvertToDto(file);
            return fileDto;
        }
    }
}
