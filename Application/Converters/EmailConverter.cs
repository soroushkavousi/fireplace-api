﻿using FireplaceApi.Application.Controllers;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Application.Converters;

public class EmailConverter : BaseConverter<Email, EmailDto>
{
    private readonly ILogger<EmailConverter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EmailConverter(ILogger<EmailConverter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public override EmailDto ConvertToDto(Email email)
    {
        if (email == null)
            return null;

        var emailDto = new EmailDto(email.Id.IdEncode(), email.UserId.IdEncode(), email.Address,
            email.Activation.Status.ToString());

        return emailDto;
    }
}
