using Microsoft.EntityFrameworkCore;

namespace ShortUrl
{
    public static class Magic
    {
        public static Action<DbContextOptionsBuilder> Setup(ConfigurationManager config)
        {
            return options =>
            {
                var key = config.GetConnectionString("DefaultConnection");
                var dbtype = config.GetConnectionString("DatabaseType");
                var conn = config.GetConnectionString(key);

                switch (dbtype)
                {
                    case "mysql":
                        {
                            options.UseMySQL(conn);
                            break;
                        }
                    case "sql":
                        {
                            options.UseMySQL(conn);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException(dbtype);
                        }
                }
            };
        }
    }

    public class Factory
    {
        ConfigurationManager config;

        public Factory(ConfigurationManager configuration)
        {
            config = configuration;
        }

        public IOpt Method()
        {
            IOpt opt;
            var key = config.GetConnectionString("DefaultConnection");
            var dbtype = config.GetConnectionString("DatabaseType");
            var conn = config.GetConnectionString(key);

            switch(dbtype)
            {
                case "mysql":
                    {
                        opt =  new Opt(conn);
                        break;
                    }
                case "sql":
                    {
                        opt =  new Opt2(conn);
                        break;
                    }
                default: {
                        throw new ArgumentOutOfRangeException(dbtype);
                    }
            }

            return opt;
        }
    }
    public interface IOpt
    {
        void CallMethodOptions(DbContextOptionsBuilder options);
    }

    public class Opt:IOpt
    {
        public string _conn;

        public Opt(string conn)
        {
            _conn = conn;
        }
        public void CallMethodOptions(DbContextOptionsBuilder options)
        {
            options.UseMySQL(_conn);
        }
    }


    public class Opt2 :IOpt
    {
        public string _conn;

        public Opt2(string conn)
        {
            _conn = conn;
        }
        public void CallMethodOptions(DbContextOptionsBuilder options)
        {
            //тут будет npgsql
            options.UseMySQL(_conn);
        }
    }
}
