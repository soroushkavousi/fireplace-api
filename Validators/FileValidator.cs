using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Controllers.Parameters.FileParameters;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamingCommunityApi.Validators
{
    public class FileValidator
    {
        private readonly ILogger<FileValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly FileRepository _fileRepository;

        public FileValidator(ILogger<FileValidator> logger, IConfiguration configuration, FileRepository fileRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _fileRepository = fileRepository;
        }

        public async Task ValidatePostFileInputParametersAsync(User requesterUser, IFormFile file)
        {
            await Task.CompletedTask;
        }
    }
}
