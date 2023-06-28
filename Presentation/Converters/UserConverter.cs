using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Presentation.Converters;

public static class UserConverter
{
    public static UserDto ToDto(this User user)
    {
        if (user == null)
            return null;

        EmailDto emailDto = null;
        if (user.Email != null)
            emailDto = user.Email.PureCopy().ToDto();

        List<SessionDto> sessionDtos = null;
        if (user.Sessions != null && user.Sessions.Count != 0)
            sessionDtos = user.Sessions.Select(
                session => session.PureCopy().ToDto()).ToList();

        var userDto = new UserDto(user.Id.IdEncode(), user.Username.Value,
            user.State.ToString(), user.Roles, user.CreationDate,
            user.DisplayName, user.About, user.AvatarUrl,
            user.BannerUrl, emailDto, sessionDtos);

        return userDto;
    }

    public static ProfileDto ToProfileDto(this User user)
    {
        if (user == null)
            return null;

        var profileDto = new ProfileDto(user.Username.Value, user.CreationDate,
            user.DisplayName, user.About, user.AvatarUrl, user.BannerUrl);

        return profileDto;
    }

    public static ProfileDto ToDto(this Profile profile)
    {
        if (profile == null)
            return null;

        var profileDto = new ProfileDto(profile.Username.Value, profile.CreationDate,
            profile.DisplayName, profile.About, profile.AvatarUrl, profile.BannerUrl);

        return profileDto;
    }
}
