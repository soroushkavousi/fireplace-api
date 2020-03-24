using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Exceptions;
using GamingCommunityApi.Core.Extensions;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Models.UserInformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Interfaces.IRepositories;

namespace GamingCommunityApi.Core.Validators
{
    public class FileValidator
    {
        private readonly ILogger<FileValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;

        public FileValidator(ILogger<FileValidator> logger, IConfiguration configuration, IFileRepository fileRepository)
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
