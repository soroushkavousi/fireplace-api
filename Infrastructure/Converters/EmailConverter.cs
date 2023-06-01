using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.ValueObjects;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters;

public class EmailConverter
{
    private readonly ILogger<EmailConverter> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EmailConverter(ILogger<EmailConverter> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    // Entity

    public EmailEntity ConvertToEntity(Email email)
    {
        if (email == null)
            return null;

        UserEntity userEntity = null;
        if (email.User != null)
            userEntity = _serviceProvider.GetService<UserConverter>()
                .ConvertToEntity(email.User.PureCopy());

        var emailEntity = new EmailEntity(email.Id, email.UserId, email.Address,
            email.Activation.Status.ToString(), email.CreationDate,
            email.ModifiedDate, email.Activation.Code, userEntity);

        return emailEntity;
    }

    public Email ConvertToModel(EmailEntity emailEntity)
    {
        if (emailEntity == null)
            return null;

        User user = null;
        if (emailEntity.UserEntity != null)
            user = _serviceProvider.GetService<UserConverter>().ConvertToModel(emailEntity.UserEntity.PureCopy());

        var activation = new Activation(emailEntity.ActivationStatus.ToEnum<ActivationStatus>(),
            emailEntity.ActivationCode);

        var email = new Email(emailEntity.Id, emailEntity.UserEntityId,
            emailEntity.Address, activation, emailEntity.CreationDate, emailEntity.ModifiedDate, user);

        return email;
    }
}
