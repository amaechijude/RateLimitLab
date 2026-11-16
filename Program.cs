using Microsoft.AspNetCore.HttpOverrides;
using RateLimitLab;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect("localhost:6379")
);

//
builder.Services.AddRedisRateLimit();

var app = builder.Build();

app.UseForwardedHeaders();

app.MapGet("/check", () => Results.Ok()).RequireIpRateLimit("check");

app.Run();
