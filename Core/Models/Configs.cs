using FireplaceApi.Core.Attributes;
using FireplaceApi.Core.Enums;
using System;

namespace FireplaceApi.Core.Models
{
    public class Configs : BaseModel
    {
        public static Configs Current { get; set; }

        public EnvironmentName EnvironmentName { get; set; }
        public ApiConfigs Api { get; set; }
        public FileConfigs File { get; set; }
        public PaginationConfigs Pagination { get; set; }
        public EmailConfigs Email { get; set; }
        public GoogleConfigs Google { get; set; }

        public Configs(ulong id, EnvironmentName environmentName, ApiConfigs api,
            FileConfigs file, PaginationConfigs pagination,
            EmailConfigs email, GoogleConfigs google, DateTime creationDate,
            DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            EnvironmentName = environmentName;
            Api = api ?? throw new ArgumentNullException(nameof(api));
            File = file ?? throw new ArgumentNullException(nameof(file));
            Pagination = pagination ?? throw new ArgumentNullException(nameof(pagination));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Google = google ?? throw new ArgumentNullException(nameof(google));
        }

        public Configs PureCopy() => new(Id, EnvironmentName, Api, File,
            Pagination, Email, Google, CreationDate, ModifiedDate);

        public class ApiConfigs
        {
            public string BaseUrlPath { get; set; }
            public int CookieMaxAgeInDays { get; set; }
        }

        public class FileConfigs
        {
            public string BasePhysicalPath { get; set; }
            public string BaseUrlPath { get; set; }
            public int GeneratedFileNameLength { get; set; }
        }

        public class PaginationConfigs
        {
            public int TotalItemsCount { get; set; }
            public int MaximumOfPageItemsCount { get; set; }
            public int GeneratedPointerLength { get; set; }
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
                CookieMaxAgeInDays = 30
            },
            file: new FileConfigs
            {
                BasePhysicalPath = "files",
                BaseUrlPath = "https://files.server.com",
                GeneratedFileNameLength = 12
            },
            pagination: new PaginationConfigs
            {
                TotalItemsCount = 300,
                MaximumOfPageItemsCount = 30,
                GeneratedPointerLength = 10
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
