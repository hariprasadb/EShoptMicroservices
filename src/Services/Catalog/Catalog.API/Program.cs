

using BuildingBlocks.Behaviors;
using JasperFx;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Add Services to container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));

});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database"));
    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
}).UseLightweightSessions();
var app = builder.Build();


//configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception == null)
        {
            return;
        }

        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Status = StatusCodes.Status500InternalServerError,
            Detail = exception.StackTrace
        };
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        logger.LogError(exception, exception.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    });
}


);

app.Run();

/*
 1. Define Model class inside Models folder
 2. Create Feature folder(Products) and subfolder CreateProduct. Add CreateProductEnpoint.cs 
        and CreateProductHandler.cs
 3. Create BuildingBlocks library project 
 4. In Building Blocks project Add MediatR library
 5. In Building blocks Project Add ICommand (iherit from IRequest), ICommandHandler(inherited from IRequestHandler)
 6. In Building blocks project add IQuery (inherit from IRequest) and IQueryHandler(inherit from IQueryHandler)
 7. CreateProudctHandler to implement the interface ICommandHandler 
 8. In CreateProductHandler.cs  
 */