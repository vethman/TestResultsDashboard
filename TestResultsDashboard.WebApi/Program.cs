using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using TestResultsDashboard.Repositories;
using TestResultsDashboard.Services;
using TestResultsDashboard.Services.JsonConverters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
await builder.Services.AddRepositoriesAsync(builder.Configuration);
builder.Services
    .AddServices()
    .AddFluentValidators();

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumWithDefaultFallbackConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c => c.MapType<TimeSpan?>(() => new OpenApiSchema { Type = "string", Example = new OpenApiString("00:00:00") }));

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