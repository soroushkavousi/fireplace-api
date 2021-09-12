using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;

namespace FireplaceApi.Core.Validators
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
