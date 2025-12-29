var builder = WebApplication.CreateBuilder(args);

//Add Services to Container
//Infrastructure - EF Core 
//Application MediatR
//API - Carter , HealthChecks

//builder.Services.AddApplicationServices() //MediatR
    //.AddInfrastructureServices(builder.Configuration)
    //.AddWebServices()
   // builder.Services



var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
