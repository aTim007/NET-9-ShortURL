public class ConnectionFactory
{
    private readonly ConfigurationManager _config;
    public ConnectionFactory(ConfigurationManager config)
    {
        _config = config;
    }

    public IOptions CreateObjectForOptions()
    {
        IOptions options;
        var databaseType = _config.GetConnectionString("DefaultConnection");
        var conn = _config.GetConnectionString(databaseType);
        switch (databaseType)
        {
            case "mysql":
                {
                    options = new MySqlOptions(conn);
                    break;
                }
            default:
                {
                    throw new ArgumentOutOfRangeException(nameof(databaseType));
                }
        }

        return options;
    }
}
