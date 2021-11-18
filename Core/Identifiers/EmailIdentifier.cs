using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Identifiers
{
    public abstract class EmailIdentifier
    {
        public static EmailIdIdentifier OfId(ulong id)
             => new EmailIdIdentifier(id);

        public static EmailAddressIdentifier OfAddress(string address)
            => new EmailAddressIdentifier(address);

        public static EmailUserIdIdentifier OfUserId(ulong userId)
            => new EmailUserIdIdentifier(userId);
    }

    public class EmailIdIdentifier : EmailIdentifier, IIdIdentifier
    {
        public ulong Id { get; set; }

        internal EmailIdIdentifier(ulong id)
        {
            Id = id;
        }
    }

    public class EmailAddressIdentifier : EmailIdentifier
    {
        public string Address { get; set; }

        internal EmailAddressIdentifier(string address)
        {
            Address = address;
        }
    }

    public class EmailUserIdIdentifier : EmailIdentifier
    {
        public ulong UserId { get; set; }

        internal EmailUserIdIdentifier(ulong userId)
        {
            UserId = userId;
        }
    }
}
