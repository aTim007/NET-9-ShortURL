using Microsoft.EntityFrameworkCore;

public class MySqlOptions : IOptions
{
    private readonly string _conn;
    public MySqlOptions(string connection)
    {
        _conn = connection;
    }

    public void CallOptionsMethod(DbContextOptionsBuilder options)
    {
        options.UseMySQL(_conn);
    }
}

