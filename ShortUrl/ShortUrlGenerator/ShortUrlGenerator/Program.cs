using Microsoft.EntityFrameworkCore;
using ShortUrlGenerator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var factoryConn = new ConnectionFactory(builder.Configuration);
builder.Services.AddDbContext<UrlContext>(options => factoryConn.CreateObjectForOptions().CallOptionsMethod(options));
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<UrlManager>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
