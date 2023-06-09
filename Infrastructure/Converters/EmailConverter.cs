using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.ValueObjects;
using FireplaceApi.Infrastructure.Entities;

namespace FireplaceApi.Infrastructure.Converters;

public static class EmailConverter
{
    public static EmailEntity ToEntity(this Email email)
    {
        if (email == null)
            return null;

        UserEntity userEntity = null;
        if (email.User != null)
            userEntity = email.User.PureCopy().ToEntity();

        var emailEntity = new EmailEntity(email.Id, email.UserId, email.Address,
            email.Activation.Status.ToString(), email.CreationDate,
            email.ModifiedDate, email.Activation.Code, userEntity);

        return emailEntity;
    }

    public static Email ToModel(this EmailEntity emailEntity)
    {
        if (emailEntity == null)
            return null;

        User user = null;
        if (emailEntity.UserEntity != null)
            user = emailEntity.UserEntity.PureCopy().ToModel();

        var activation = new Activation(emailEntity.ActivationStatus.ToEnum<ActivationStatus>(),
            emailEntity.ActivationCode);

        var email = new Email(emailEntity.Id, emailEntity.UserEntityId,
            emailEntity.Address, activation, emailEntity.CreationDate, emailEntity.ModifiedDate, user);

        return email;
    }
}
