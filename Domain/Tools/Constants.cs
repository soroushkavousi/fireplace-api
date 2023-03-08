using FireplaceApi.Domain.Enums;

namespace FireplaceApi.Domain.Tools
{
    public static class Constants
    {
        public static string SecretsDirectoryPath { get; } = "Secrets";
        public static SortType DefaultSort { get; } = SortType.TOP;
    }
}
