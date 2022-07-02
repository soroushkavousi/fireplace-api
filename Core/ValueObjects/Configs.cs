namespace FireplaceApi.Core.ValueObjects
{
    public class Configs
    {
        public static Configs Instance { get; set; }

        public DatabaseConfigs Database { get; set; }
        public LogConfigs Log { get; set; }
        public ApiConfigs Api { get; set; }
        public FileConfigs File { get; set; }
        public PaginationConfigs Pagination { get; set; }
        public EmailConfigs Email { get; set; }
        public GoogleConfigs Google { get; set; }

        public class DatabaseConfigs
        {
            public string ConnectionString { get; set; }
        }

        public class LogConfigs
        {
            public string ConfigFilePath { get; set; }
            public string RootDirectoryPath { get; set; }
        }

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
            public string ActivationMessageFormat { get; set; }
            public string ActivationSubject { get; set; }
        }

        public class GoogleConfigs
        {
            public string BaseAuthUrl { get; set; }
            public string BaseTokenUrl { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string RelativeRedirectUrl { get; set; }
        }
    }
}
