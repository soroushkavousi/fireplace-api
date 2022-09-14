using FireplaceApi.Core.Models;
using System;

namespace FireplaceApi.Infrastructure.ValueObjects
{
    public class ConfigsEntityData
    {
        public Configs.ApiConfigs Api { get; set; }
        public Configs.FileConfigs File { get; set; }
        public Configs.PaginationConfigs Pagination { get; set; }
        public Configs.EmailConfigs Email { get; set; }
        public Configs.GoogleConfigs Google { get; set; }

        public ConfigsEntityData(Configs.ApiConfigs api,
            Configs.FileConfigs file, Configs.PaginationConfigs pagination,
            Configs.EmailConfigs email, Configs.GoogleConfigs google)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            File = file ?? throw new ArgumentNullException(nameof(file));
            Pagination = pagination ?? throw new ArgumentNullException(nameof(pagination));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Google = google ?? throw new ArgumentNullException(nameof(google));
        }
    }
}
