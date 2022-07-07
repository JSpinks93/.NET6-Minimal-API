// This is a basic example of a dotnet core minimal API
// There are various examples throughout this code marked by comments
// Additional info on minimal api fundamentals can be found here: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0

// This can be tested through swagger by running the api ('dotnet run' command)
// Swagger endpoint will be http://localhost:{port}/swagger

using Microsoft.OpenApi.Models; // Required for Swagger code
using PizzaStore.DB; // Reference to our Db class
using PizzaStore.Services.DependencyInjectionExampleService; // Reference to our DI class

#region Minimal API Base Code

var builder = WebApplication.CreateBuilder(args);

// Add Service for dependency injection
AddServiceForDependencyInjection();

// Swagger Step 1 of 2 - Add documentation with Swagger
AddSwaggerDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Swagger Step 2 of 2 - Add Swagger Middleware
AddSwaggerMiddleware();

// Add endpoints for Pizza API
RegisterPizzaAPI();

// Add endpoint with example of pulling value from configuration
RegisterConfigValueExampleAPI();

// Add endpoint leveraging dependency injected service
RegisterDependencyInjectedServiceAPI();

// Basic Logging
app.Logger.LogInformation("App started!");

app.Run();

#endregion

#region Local Functions

void AddServiceForDependencyInjection()
{
    builder.Services.AddScoped<IDependencyInjectionExampleService, DependencyInjectionExampleService>();
}

void AddSwaggerDocumentation()
{
    // **Requires installing Swashbuckle.AspNetCore**
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(x =>
    {
        var version = "v1";

        x.SwaggerDoc(version, new OpenApiInfo 
        { 
            Title = "Todo API",
            Description = "Keep track of your tasks",
            Version = version
        });
    });
}

void AddSwaggerMiddleware()
{
    // **Requires installing Swashbuckle.AspNetCore**
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
    });
}

void RegisterPizzaAPI()
{
    app.MapGet("/pizzas/{id}", (int id) => PizzaDB.GetPizza(id));

    app.MapGet("/pizzas", () => PizzaDB.GetPizzas());

    app.MapPost("/pizzas", (Pizza pizza) => PizzaDB.CreatePizza(pizza));

    app.MapPut("/pizzas", (Pizza pizza) => PizzaDB.UpdatePizza(pizza));

    app.MapDelete("/pizzas/{id}", (int id) => PizzaDB.RemovePizza(id));
}

void RegisterConfigValueExampleAPI()
{
    // Pulls value from appsettings.json if it exists, otherwise uses a default value
    var configValue = app.Configuration["TestConfigValue"] ?? "No config value found...";

    app.MapGet("/configValue", () => configValue);
}

void RegisterDependencyInjectedServiceAPI()
{
    app.MapGet("/dependencyInjectedRandomNumber", () =>
    {
        using (var scope = app.Services.CreateScope())
        {
            IResult result;

            var diService = scope.ServiceProvider.GetService<IDependencyInjectionExampleService>();

            if (diService == null)
            {
                app.Logger.LogError("DI Service was not registered properly and returned null.");

                result =  Results.Problem("Unable to retrieve random number! Please try again later.");
            }
            else
            {
                result = Results.Ok(diService.GetRandomNumber());
            }

            return result;
        }
    });
}

#endregion
