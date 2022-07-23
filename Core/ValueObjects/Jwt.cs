using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;

namespace FireplaceApi.Core.ValueObjects
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
#pragma warning disable CS0618 // Type or member is obsolete
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
#pragma warning restore CS0618 // Type or member is obsolete
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
