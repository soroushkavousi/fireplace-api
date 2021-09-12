﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    public class KebabCaseNamingPolicy : JsonNamingPolicy
    {
        private readonly SnakeCaseNamingStrategy _newtonsoftSnakeCaseNamingStrategy =  new SnakeCaseNamingStrategy();

        public static KebabCaseNamingPolicy Instance { get; } = new KebabCaseNamingPolicy();

        public override string ConvertName(string name)
        {
            return _newtonsoftSnakeCaseNamingStrategy.GetPropertyName(name, false).Replace("_", "-");
        }
    }
}
