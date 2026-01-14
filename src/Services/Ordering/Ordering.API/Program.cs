using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add Services to Container
//Infrastructure - EF Core 
//Application MediatR
////API - Carter , HealthChecks
builder.Services.AddApplicationServices(builder.Configuration)
   .AddInfrastructureServices(builder.Configuration)
   .AddApiServices(builder.Configuration);





var app = builder.Build();
app.UseApiServices();
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
//app.MapGet("/", () => "Hello World!");

app.Run();
//docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d 