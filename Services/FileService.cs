using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using GamingCommunityApi.Gateways;
using GamingCommunityApi.Models;
using GamingCommunityApi.Models.UserInformations;
using GamingCommunityApi.Operators;
using GamingCommunityApi.Repositories;
using GamingCommunityApi.Tools;
using GamingCommunityApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GamingCommunityApi.Services
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
