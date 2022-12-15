
using Microsoft.EntityFrameworkCore;

public interface IOptions
{
    void CallOptionsMethod(DbContextOptionsBuilder options);
}