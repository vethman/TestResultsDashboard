using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Entities;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Repositories.Configuration;
using TestResultsDashboard.Services;
using TestResultsDashboard.Services.JsonConverters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRepositories()
    .AddServices()
    .AddFluentValidators();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumWithDefaultFallbackConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => c.MapType<TimeSpan?>(() => new OpenApiSchema { Type = "string", Example = new OpenApiString("00:00:00") }));

var databaseConfig = new DatabaseConfig();
builder.Configuration.GetSection("Database").Bind(databaseConfig);

await DB.InitAsync(databaseConfig.Name, databaseConfig.Host, databaseConfig.Port);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }