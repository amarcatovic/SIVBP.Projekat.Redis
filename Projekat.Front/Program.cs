using Microsoft.EntityFrameworkCore;
using Projekat.Front.Controllers;
using Projekat.Front.Infrastructure.Caching;
using Projekat.Front.Infrastructure.Persistence;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
     .AddApplicationPart(typeof(WeatherForecastController).Assembly)
     .AddApplicationPart(typeof(PostsController).Assembly)
                .AddControllersAsServices();

builder.Services.AddDbContext<StackOverflow2010Context>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), providerOptions =>
    {
        providerOptions.CommandTimeout(180);
    }));

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")));
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

app.MapFallbackToFile("index.html"); ;

app.Run();
