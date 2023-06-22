using FireplaceApi.Application.Models;
using System;

namespace FireplaceApi.Infrastructure.ValueObjects;

public class ConfigsEntityData
{
    public Configs.ApiConfigs Api { get; set; }
    public Configs.FileConfigs File { get; set; }
    public Configs.QueryResultConfigs QueryResult { get; set; }
    public Configs.EmailConfigs Email { get; set; }
    public Configs.GoogleConfigs Google { get; set; }

    public ConfigsEntityData(Configs.ApiConfigs api,
        Configs.FileConfigs file, Configs.QueryResultConfigs queryResult,
        Configs.EmailConfigs email, Configs.GoogleConfigs google)
    {
        Api = api ?? throw new ArgumentNullException(nameof(api));
        File = file ?? throw new ArgumentNullException(nameof(file));
        QueryResult = queryResult ?? throw new ArgumentNullException(nameof(queryResult));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Google = google ?? throw new ArgumentNullException(nameof(google));
    }
}
