using System;

namespace FireplaceApi.Core.ValueObjects
{
    public class GlobalValues
    {
        public ApiGlobalValues Api { get; set; }
        public LogGlobalValues Log { get; set; }
        public PaginationGlobalValues Pagination { get; set; }
        public FileGlobalValues File { get; set; }
        public EmailGlobalValues Email { get; set; }
        public GoogleGlobalValues Google { get; set; }


        private GlobalValues() { }

        public GlobalValues(LogGlobalValues log, FileGlobalValues file,
            EmailGlobalValues email, GoogleGlobalValues google)
        {
            Log = log ?? throw new ArgumentNullException(nameof(log));
            File = file ?? throw new ArgumentNullException(nameof(file));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Google = google ?? throw new ArgumentNullException(nameof(google));
        }

        public class ApiGlobalValues
        {
            public string BaseUrlPath { get; set; }

            private ApiGlobalValues() { }

            public ApiGlobalValues(string baseUrlPath)
            {
                BaseUrlPath = baseUrlPath ?? throw new ArgumentNullException(nameof(baseUrlPath));
            }
        }

        public class LogGlobalValues
        {
            public string ConfigFilePath { get; set; }
            public string RootDirectoryPath { get; set; }

            private LogGlobalValues() { }

            public LogGlobalValues(string configFilePath, string rootDirectoryPath)
            {
                ConfigFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
                RootDirectoryPath = rootDirectoryPath ?? throw new ArgumentNullException(nameof(rootDirectoryPath));
            }
        }

        public class PaginationGlobalValues
        {
            public int TotalItemsCount { get; set; }
            public int MaximumOfPageItemsCount { get; set; }
            public int GeneratedPointerLength { get; set; }

            private PaginationGlobalValues() { }

            public PaginationGlobalValues(int totalItemsCount, int maximumOfPageItemsCount,
                int generatedPointerLength)
            {
                TotalItemsCount = totalItemsCount;
                MaximumOfPageItemsCount = maximumOfPageItemsCount;
                GeneratedPointerLength = generatedPointerLength;
            }
        }

        public class FileGlobalValues
        {
            public string BasePhysicalPath { get; set; }
            public string BaseUrlPath { get; set; }
            public int GeneratedFileNameLength { get; set; }

            private FileGlobalValues() { }

            public FileGlobalValues(string basePhysicalPath, string baseUrlPath,
                int generatedFileNameLength)
            {
                BasePhysicalPath = basePhysicalPath ?? throw new ArgumentNullException(nameof(basePhysicalPath));
                BaseUrlPath = baseUrlPath ?? throw new ArgumentNullException(nameof(baseUrlPath));
                GeneratedFileNameLength = generatedFileNameLength;
            }
        }

        public class EmailGlobalValues
        {
            public string Address { get; set; }
            public string Password { get; set; }
            public string SmtpServerAddress { get; set; }
            public int SmtpServerPort { get; set; }
            public string ActivationMessageFormat { get; set; }
            public string ActivationSubject { get; set; }

            private EmailGlobalValues() { }

            public EmailGlobalValues(string address, string password, string smtpServerAddress,
                int smtpServerPort, string activationMessageFormat, string activationSubject)
            {
                Address = address ?? throw new ArgumentNullException(nameof(address));
                Password = password ?? throw new ArgumentNullException(nameof(password));
                SmtpServerAddress = smtpServerAddress ?? throw new ArgumentNullException(nameof(smtpServerAddress));
                SmtpServerPort = smtpServerPort;
                ActivationMessageFormat = activationMessageFormat ?? throw new ArgumentNullException(nameof(activationMessageFormat));
                ActivationSubject = activationSubject ?? throw new ArgumentNullException(nameof(activationSubject));
            }
        }

        public class GoogleGlobalValues
        {
            public string BaseAuthUrl { get; set; }
            public string BaseTokenUrl { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string RelativeRedirectUrl { get; set; }

            private GoogleGlobalValues() { }

            public GoogleGlobalValues(string baseAuthUrl, string baseTokenUrl,
                string clientId, string clientSecret, string relativeRedirectUrl)
            {
                BaseAuthUrl = baseAuthUrl ?? throw new ArgumentNullException(nameof(baseAuthUrl));
                BaseTokenUrl = baseTokenUrl ?? throw new ArgumentNullException(nameof(baseTokenUrl));
                ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
                ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
                RelativeRedirectUrl = relativeRedirectUrl ?? throw new ArgumentNullException(nameof(relativeRedirectUrl));
            }
        }
    }
}

//public string ApiEmailAddress { get; set; }
//public string ApiEmailPassword { get; set; }
//public string ApiEmailSmtpServerAddress { get; set; }
//public int ApiEmailSmtpServerPort { get; set; }
//public string EmailActivationMessageFormat { get; set; }
//public string EmailActivationSubject { get; set; }
//public string GoogleClientId { get; set; }
//public string GoogleClientSecret { get; set; }
