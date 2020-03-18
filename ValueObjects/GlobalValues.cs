using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.ValueObjects
{
    public class GlobalValues
    {
        public string SignUpWithEmailMessageFormat { get; set; }

        private GlobalValues() { }

        public GlobalValues(string signUpWithEmailMessageFormat)
        {
            SignUpWithEmailMessageFormat = signUpWithEmailMessageFormat ?? throw new ArgumentNullException(nameof(signUpWithEmailMessageFormat));
        }
    }
}
