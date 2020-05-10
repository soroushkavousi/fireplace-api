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
using GamingCommunityApi.Core.Operators;

namespace GamingCommunityApi.Core.Validators
{
    public class FileValidator
    {
        private readonly ILogger<FileValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly FileOperator _fileOperator;

        public FileValidator(ILogger<FileValidator> logger, IConfiguration configuration, FileOperator fileOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _fileOperator = fileOperator;
        }

        public async Task ValidatePostFileInputParametersAsync(User requesterUser, IFormFile file)
        {
            await Task.CompletedTask;
        }
    }
}
