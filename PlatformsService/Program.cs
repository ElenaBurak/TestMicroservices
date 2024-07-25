using E3.PlatformService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PlatformsService.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformsService", Version = "v1" });
});

var app = builder.Build();

Console.WriteLine($"--> CommandService Endpoint {app.Configuration["CommandService"]}");
// foreach (var kvp in app.Configuration.AsEnumerable())
// {
//     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
// }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
//app.UseAuthorization();
app.MapControllers();
PrepDb.PrepPopulation(app);
app.Run();