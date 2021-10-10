using FireplaceApi.Core.Enums;
using FireplaceApi.Core.ValueObjects;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Core.Models
{
    public class User : BaseModel
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
            string username, UserState state, DateTime creationDate,
            DateTime? modifiedDate = null, Password password = null, Email email = null,
            GoogleUser googleUser = null, List<AccessToken> accessTokens = null,
            List<Session> sessions = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            FirstName = firstName ?? throw new ArgumentNullException(nameof(username));
            LastName = lastName ?? throw new ArgumentNullException(nameof(username));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            State = state;
            CreationDate = creationDate;
            Password = password;
            Email = email;
            GoogleUser = googleUser;
            AccessTokens = accessTokens;
            Sessions = sessions;
        }


        public User PureCopy() => new User(Id, FirstName, LastName,
                Username, State, CreationDate, ModifiedDate, Password);

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