using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GamingCommunityApi.Core.ValueObjects
{
    public class Jwt
    {
        private readonly static IJwtDecoder _decoder;
        public string Content { get; set; }
        public string PayloadString { set; get; }

        static Jwt()
        {
            _decoder = InitializeDecoder();
        }

        private static IJwtDecoder InitializeDecoder()
        {
            IJsonSerializer serializer = new JsonNetSerializer();
            var provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            return new JwtDecoder(serializer, validator, urlEncoder, algorithm);
        }

        public Jwt(string content)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public T ExtractPayload<T>()
        {
            return _decoder.DecodeToObject<T>(Content);
        }
    }
}
