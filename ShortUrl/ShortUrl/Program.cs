using ShortUrl.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//подключение БД
//string connection = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<URLContext>(options => options.UseMySQL(connection));

// Add services to the container.
builder.Services.AddMvc(options =>
{
	options.EnableEndpointRouting = false;
});
//builder.Services.AddRazorPages();

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

//app.MapRazorPages();

app.Run();
