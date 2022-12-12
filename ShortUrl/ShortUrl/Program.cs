
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShortUrl;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UrlContext>(options => options.UseMySQL(connection));

builder.Services.AddScoped<UrlManager>();
builder.Services.AddScoped<HashManager>();

// Add services to the container.
builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

//builder.Services.AddRazorPages();

//Кодировка заголовка исправлена на UTF-8 для избежания ошибок при наличии кириллицы в запросе
builder.WebHost.ConfigureKestrel(options =>
{
    options.RequestHeaderEncodingSelector = (_) => Encoding.UTF8;
    options.ResponseHeaderEncodingSelector = (_) => Encoding.UTF8;

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseAuthorization();

app.UseRouting();
app.UseMvc(options =>
{
    options.MapRoute(
                    "Default",
                    "{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = "" }
                    );
});

app.Run();