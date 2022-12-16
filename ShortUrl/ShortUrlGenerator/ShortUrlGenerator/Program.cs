using Microsoft.EntityFrameworkCore;
using ShortUrlGenerator;
using ShortUrlGenerator.Controllers;
using System;
using System.Text;
using Crc32 = System.IO.Hashing.Crc32;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Options;

Random _random = new();
 string GetRandomString()
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string(Enumerable.Repeat(chars, _random.Next(0, 100))
        .Select(s => s[_random.Next(s.Length)]).ToArray());
}

static void ChangeHash(ref byte[] bytes)
{
    for (int i = bytes.Length - 1; i >= 0; i--) //???
    {
        if (bytes[i] < byte.MaxValue)
        {
            bytes[i]++;
            return;
        }
        bytes[i] = 0;
    }
    //Array.Resize(ref bytes, bytes.Length+1);
    //bytes[0] = 1;
}
byte[] bytes = { 255, 254, 255 };
while (true)
    ChangeHash(ref bytes);
return;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var factoryConn = new ConnectionFactory(builder.Configuration);
builder.Services.AddDbContext<UrlContext>(options => factoryConn.CreateObjectForOptions().CallOptionsMethod(options));
builder.Services.AddScoped<IUrlRepository<URL>, UrlRepository>();
builder.Services.AddScoped<UrlManager>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
