

using Basket.API.Data;
using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter();
// Add Services to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddStackExchangeRedisCache((options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
}));

builder.Services.AddMediatR((configuration) =>
{
    configuration.RegisterServicesFromAssembly(assembly);
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
}
);
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketRepository>((serviceProvider) =>
{
   var basketRepo= serviceProvider.GetRequiredService<BasketRepository>();
   var distCache = serviceProvider.GetRequiredService<IDistributedCache>();         
    return new CachedBasketRepository(basketRepo, distCache);  

});
builder.Services.AddScoped<IBasketRepository,BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddHealthChecks()
  .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
  .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
    
var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(Options => { });
app.UseHealthChecks("/health",new HealthCheckOptions 
                              {ResponseWriter= UIResponseWriter.WriteHealthCheckUIResponse } 
                    );



app.Run();
