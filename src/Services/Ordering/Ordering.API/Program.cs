using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Add Services to Container
//Infrastructure - EF Core 
//Application MediatR
////API - Carter , HealthChecks
builder.Services.AddApplicationServices()
   .AddInfrastructureServices(builder.Configuration)
   .AddApiServices();




var app = builder.Build();
app.UseApiServices();
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
//app.MapGet("/", () => "Hello World!");

app.Run();
