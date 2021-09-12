﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Api.Controllers;

namespace FireplaceApi.Api.Converters
{
    public class EmailConverter
    {
        private readonly ILogger<EmailConverter> _logger;
        private readonly IServiceProvider _serviceProvider;

        public EmailConverter(ILogger<EmailConverter> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public EmailDto ConvertToDto(Email email)
        {
            if (email == null)
                return null;

            UserDto userDto = null;
            if (email.User != null)
                userDto = _serviceProvider.GetService<UserConverter>().ConvertToDto(email.User.PureCopy());

            var emailDto = new EmailDto(email.Id, email.UserId, email.Address,
                email.Activation.Status.ToString(), userDto);

            return emailDto;
        }
    }
}
