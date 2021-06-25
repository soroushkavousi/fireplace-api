using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using GamingCommunityApi.Core.Tools.NewtonsoftSerializer;

namespace GamingCommunityApi.Core.Extensions
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
