using FireplaceApi.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Domain.ValueObjects
{
    public abstract class Enumeration<TEnum> where TEnum : Enumeration<TEnum>
    {
        public string Name { get; }

        private static readonly Dictionary<string, TEnum> _dictionary = new();
        public static List<string> List => _dictionary.Keys.ToList();

        protected Enumeration([CallerMemberName] string name = null)
        {
            Name = name;
            _dictionary.Add(name, (TEnum)this);
        }

        public override string ToString() => Name;

        public static TEnum FromName(string name)
        {
            _dictionary.TryGetValue(name.ToUpper(), out var matchingItem);

            if (matchingItem == null)
            {
                throw new InternalServerException("Enum type doesn't have this name.", new { Enum = typeof(TEnum), name });
            }

            return matchingItem;
        }
    }
}
