

using JasperFx;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Add Services to container
builder.Services.AddCarter();
builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);

});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database"));
    opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
}).UseLightweightSessions();
var app = builder.Build();


//configure the HTTP request pipeline
app.MapCarter();

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