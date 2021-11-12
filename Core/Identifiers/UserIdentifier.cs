using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Identifiers
{
    public abstract class UserIdentifier
    {
        public static UserIdIdentifier OfId(ulong id)
        {
            return new UserIdIdentifier(id);
        }

        public static UserUsernameIdentifier OfUsername(string username)
        {
            return new UserUsernameIdentifier(username);
        }
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
