using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Core.ValueObjects
{
    public class Password
    {
        private string _hash;
        public string Value { get; set; }
        public string Hash { get => _hash ?? ComputeHashOfPasswordValue(Value); set => _hash = value; }

        private Password(string value = null, string hash = null)
        {
            Value = value;
            Hash = hash;
        }

        public static Password OfValue(string passwordValue)
        {
            if (passwordValue == null)
                return null;
            return new Password(value: passwordValue);
        }

        public static Password OfHash(string passwordHash)
        {
            if (passwordHash == null)
                return null;
            return new Password(hash: passwordHash);
        }

        private string ComputeHashOfPasswordValue(string password)
        {
            _hash = password;
            return _hash;
        }
    }
}
