using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.ValueObjects
{
    public class GlobalValues
    {
        public string ApiEmailAddress { get; set; }
        public string ApiEmailPassword { get; set; }
        public string ApiEmailSmtpServerAddress { get; set; }
        public int ApiEmailSmtpServerPort { get; set; }
        public string EmailActivationMessageFormat { get; set; }
        public string EmailActivationSubject { get; set; }


        private GlobalValues() { }

        public GlobalValues(string apiEmailAddress, string apiEmailPassword, 
            string apiEmailSmtpServerAddress, int apiEmailSmtpServerPort, 
            string emailActivationMessageFormat, string emailActivationSubject)
        {
            ApiEmailAddress = apiEmailAddress ?? throw new ArgumentNullException(nameof(apiEmailAddress));
            ApiEmailPassword = apiEmailPassword ?? throw new ArgumentNullException(nameof(apiEmailPassword));
            ApiEmailSmtpServerAddress = apiEmailSmtpServerAddress ?? throw new ArgumentNullException(nameof(apiEmailSmtpServerAddress));
            ApiEmailSmtpServerPort = apiEmailSmtpServerPort;
            EmailActivationMessageFormat = emailActivationMessageFormat ?? throw new ArgumentNullException(nameof(emailActivationMessageFormat));
            EmailActivationSubject = emailActivationSubject ?? throw new ArgumentNullException(nameof(emailActivationSubject));
        }
    }
}
