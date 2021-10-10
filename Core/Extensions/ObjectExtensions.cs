using FireplaceApi.Core.Tools;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace FireplaceApi.Core.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings;
        public static JavaScriptEncoder AllUnicodeRanges = JavaScriptEncoder.Create(UnicodeRanges.All, UnicodeRanges.All);

        static ObjectExtensions()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = CoreContractResolver.Instance,
                //NullValueHandling = NullValueHandling.Ignore,
            };
            _jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            _jsonSerializerSettings.Converters.Add(new IPAddressConverter());
            _jsonSerializerSettings.Converters.Add(new IPEndPointConverter());
        }

        public static string ToJson(this object obj)
        {
            if (obj == null)
                return null;
            return JsonConvert.SerializeObject(obj, _jsonSerializerSettings);
        }

        //public static string ToJson(this object obj)
        //{
        //    //Todo
        //    //Accept persian unicodes
        //    //var encoderSettings = new TextEncoderSettings();
        //    //encoderSettings.AllowCharacters('\u0436', '\u0430');

        //    var serializeOptions = new JsonSerializerOptions
        //    {
        //        PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance,
        //        Encoder = AllUnicodeRanges,
        //        MaxDepth = 50,

        //    };
        //    return JsonSerializer.Serialize(obj, serializeOptions);
        //}

        public static DestinationType To<DestinationType>(this object obj)
        {
            if (obj == null)
                return default;
            return (DestinationType)obj;
        }
    }
}
