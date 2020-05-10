using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Core.Models.UserInformations
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public UserState State { get; set; }
        public Password Password { get; set; }
        public Email Email { get; set; }
        public GoogleUser GoogleUser { get; set; }
        public List<AccessToken> AccessTokens { get; set; }
        public List<Session> Sessions { get; set; }

        public User(long id, string firstName, string lastName,
            string username, UserState state, Password password = null,
            Email email = null, GoogleUser googleUser = null,
            List<AccessToken> accessTokens = null, List<Session> sessions = null)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(username));
            LastName = lastName ?? throw new ArgumentNullException(nameof(username));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            State = state;
            Password = password;
            Email = email;
            GoogleUser = googleUser;
            AccessTokens = accessTokens;
            Sessions = sessions;
        }


        public User PureCopy() => new User(Id, FirstName, LastName,
                Username, State, Password, null, null,
                null, null);

        public void RemoveLoopReferencing()
        {
            Email = Email?.PureCopy();
            GoogleUser = GoogleUser?.PureCopy();
            AccessTokens?.ForEach(
                accessToken => accessToken?.PureCopy());
            Sessions?.ForEach(
                session => session?.PureCopy());
        }
    }
}