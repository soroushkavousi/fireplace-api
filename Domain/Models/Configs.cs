using FireplaceApi.Domain.Attributes;
using FireplaceApi.Domain.Enums;
using System;

namespace FireplaceApi.Domain.Models
{
    public class Configs : BaseModel
    {
        public static Configs Current { get; set; }

        public EnvironmentName EnvironmentName { get; set; }
        public ApiConfigs Api { get; set; }
        public FileConfigs File { get; set; }
        public QueryResultConfigs QueryResult { get; set; }
        public EmailConfigs Email { get; set; }
        public GoogleConfigs Google { get; set; }

        public Configs(ulong id, EnvironmentName environmentName, ApiConfigs api,
            FileConfigs file, QueryResultConfigs queryResult,
            EmailConfigs email, GoogleConfigs google, DateTime creationDate,
            DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            EnvironmentName = environmentName;
            Api = api ?? throw new ArgumentNullException(nameof(api));
            File = file ?? throw new ArgumentNullException(nameof(file));
            QueryResult = queryResult ?? throw new ArgumentNullException(nameof(queryResult));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Google = google ?? throw new ArgumentNullException(nameof(google));
        }

        public Configs PureCopy() => new(Id, EnvironmentName, Api, File,
            QueryResult, Email, Google, CreationDate, ModifiedDate);

        public class ApiConfigs
        {
            public string BaseUrlPath { get; set; }
            public int CookieMaxAgeInDays { get; set; }
            public int RequestLimitionPeriodInMinutes { get; set; }
            public int MaxRequestPerIP { get; set; }
        }

        public class FileConfigs
        {
            public string BasePhysicalPath { get; set; }
            public string BaseUrlPath { get; set; }
            public int GeneratedFileNameLength { get; set; }
        }

        public class QueryResultConfigs
        {
            public int TotalLimit { get; set; }
            public int ViewLimit { get; set; }
            public int DepthLimit { get; set; }
        }

        public class EmailConfigs
        {
            public string ActivationSubject { get; set; }
            public string ActivationMessageFormat { get; set; }
            [Sensitive]
            public ValueObjects.TokenResponse GmailTokenResponse { get; set; }
        }

        public class GoogleConfigs
        {
            [Sensitive]
            public string ClientId { get; set; }
            [Sensitive]
            public string ClientSecret { get; set; }
            public string RelativeRedirectUrl { get; set; }
        }

        public static Configs Default { get; } = new(
            id: 0,
            environmentName: EnvironmentName.DEVELOPMENT,
            api: new ApiConfigs
            {
                BaseUrlPath = "https://api.server.com",
                CookieMaxAgeInDays = 30,
                RequestLimitionPeriodInMinutes = 60,
                MaxRequestPerIP = 50,
            },
            file: new FileConfigs
            {
                BasePhysicalPath = "files",
                BaseUrlPath = "https://files.server.com",
                GeneratedFileNameLength = 12
            },
            queryResult: new QueryResultConfigs
            {
                TotalLimit = 300,
                ViewLimit = 30,
                DepthLimit = 5
            },
            email: new EmailConfigs
            {
                ActivationSubject = "Project Email Activation",
                ActivationMessageFormat = "Congratulations! Code: {0}"
            },
            google: new GoogleConfigs
            {
                ClientId = "client-id",
                ClientSecret = "client-secret",
                RelativeRedirectUrl = "/users/log-in-with-google",
            },
            creationDate: DateTime.UtcNow
        );
    }
}
