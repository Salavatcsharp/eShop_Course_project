using Ordering.Api;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApiServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();


var app = builder.Build();


app.Run();