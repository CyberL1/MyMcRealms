using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using MyMcRealms.Data;
using MyMcRealms.Helpers;
using MyMcRealms.Middlewares;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();

if (Environment.GetEnvironmentVariable("CONNECTION_STRING") == null)
{
    Console.WriteLine("CONNECTION_STRING environment variable missing");
    return;
}

if (Environment.GetEnvironmentVariable("MYMC_API_KEY") == null)
{
    Console.WriteLine("MYMC_API_KEY environment variable missing");
    return;
}

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDirectoryBrowser();

var dataSourceBuilder = new NpgsqlDataSourceBuilder(Environment.GetEnvironmentVariable("CONNECTION_STRING"));
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(dataSource);
});

var app = builder.Build();

// Initialize database
Database.Initialize(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MinecraftCookieMiddleware>();

app.MapControllers();
app.UseStaticFiles();

var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "public"));
var requestPath = "/public";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = requestPath
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = fileProvider,
    RequestPath = requestPath
});

app.Run();
