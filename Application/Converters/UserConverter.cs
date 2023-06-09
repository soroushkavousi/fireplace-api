using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Application.Converters;

public static class UserConverter
{
    public static UserDto ToDto(this User user)
    {
        if (user == null)
            return null;

        EmailDto emailDto = null;
        if (user.Email != null)
            emailDto = user.Email.PureCopy().ToDto();

        string accessTokenValue = null;
        if (user.AccessTokens != null && user.AccessTokens.Count != 0)
            accessTokenValue = user.AccessTokens.Last().Value;

        List<SessionDto> sessionDtos = null;
        if (user.Sessions != null && user.Sessions.Count != 0)
            sessionDtos = user.Sessions.Select(
                session => session.PureCopy().ToDto()).ToList();

        var userDto = new UserDto(user.Id.IdEncode(), user.Username, user.State.ToString(),
            user.CreationDate, user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl,
            accessTokenValue, emailDto, sessionDtos);

        return userDto;
    }

    public static ProfileDto ToProfileDto(this User user)
    {
        if (user == null)
            return null;

        var profileDto = new ProfileDto(user.Username, user.CreationDate,
            user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl);

        return profileDto;
    }

    public static ProfileDto ToDto(this Profile profile)
    {
        if (profile == null)
            return null;

        var profileDto = new ProfileDto(profile.Username, profile.CreationDate,
            profile.DisplayName, profile.About, profile.AvatarUrl, profile.BannerUrl);

        return profileDto;
    }
}
