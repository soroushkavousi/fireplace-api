using FireplaceApi.Domain.Interfaces;

namespace FireplaceApi.Domain.Identifiers
{
    public abstract class UserIdentifier
    {
        public static UserIdIdentifier OfId(ulong id)
            => new UserIdIdentifier(id);

        public static UserUsernameIdentifier OfUsername(string username)
            => new UserUsernameIdentifier(username);
    }

    public class UserIdIdentifier : UserIdentifier, IIdIdentifier
    {
        public ulong Id { get; set; }

        internal UserIdIdentifier(ulong id)
        {
            Id = id;
        }
    }

    public class UserUsernameIdentifier : UserIdentifier
    {
        public string Username { get; set; }

        internal UserUsernameIdentifier(string username)
        {
            Username = username;
        }
    }
}
