using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GamingCommunityApi.Entities.UserInformationEntities;
using GamingCommunityApi.Enums;
using GamingCommunityApi.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GamingCommunityApi.Extensions;

namespace GamingCommunityApi.Models.UserInformations
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public Password Password { get; set; }
        public UserState State { get; set; }
        public Email Email { get; set; }
        public List<AccessToken> AccessTokens { get; set; }
        public List<Session> Sessions { get; set; }

        public User(long id, string firstName, string lastName,
            string username, Password password, UserState state,
            Email email = null,
            List<AccessToken> accessTokens = null, List<Session> sessions = null)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(username));
            LastName = lastName ?? throw new ArgumentNullException(nameof(username));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
            State = state;
            Email = email;
            AccessTokens = accessTokens;
            Sessions = sessions;
        }


        public User PureCopy() => new User(Id, FirstName, LastName,
                Username, Password, State, null,
                null, null);

        public User Copy(bool deep = false)
        {
            var copy = new User(Id, FirstName, LastName,
                Username, Password, State, Email,
                AccessTokens, Sessions);
            return copy;
        }

        public void RemoveLoopReferencing()
        {
            var pureUser = new User(Id, FirstName, LastName,
                Username, Password, State, null,
                null, null);

            if (Email != null && Email.User != null)
                Email.User = pureUser;

            if (AccessTokens.IsNullOrEmpty() == false)
            {
                AccessTokens.ForEach(
                    accessToken => accessToken.User = pureUser);
            }

            if (Sessions.IsNullOrEmpty() == false)
            {
                Sessions.ForEach(
                    session => session.User = pureUser);
            }
        }
    }
}