using MongoExample.Models;
using MongoExample.Services;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
// Add MongoDBService as a singleton
builder.Services.AddSingleton<MongoDBService>();
// Add controllers to the service container
builder.Services.AddControllers();

// Add Swagger for API documentation and testing with custom configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pharmaceutical Herbs API",
        Version = "v1",
        Description = "A RESTful API for herbs. Search for a Herb and check the pharmaceutical data. This data is from the European Medicines Agency.",
        Contact = new OpenApiContact
        {
            Name = "Designed By Cyprain Chidozie",
            Email = "Cyprainchidozie232@gmail.com",
            Url = new Uri("https://cypso05.github.io/softwaredev/") // Contact URL for more info
        }
    });

    // Optional: If you use XML documentation, you can include it as well.
    // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "PharmaceuticalHerbs.xml"));
});

var app = builder.Build();

// Enable CORS for testing (you can tighten the policy later)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); // Allow all origins for testing
    });
});

// Enable routing and apply CORS policy
app.UseRouting();
app.UseCors();

// Log when the app is starting
app.Logger.LogInformation("Application started...");

try
{
    // Configure the HTTP request pipeline for Swagger
    app.UseSwagger(); // Enable Swagger UI for all environments
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PharmaHerbs API v1");
        options.DocumentTitle = "PharmaHerbs API Documentation - Data from EMA"; // Title for the Swagger UI page
        options.RoutePrefix = "swagger"; // Make Swagger UI accessible at /swagger
    });
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "An error occurred while setting up Swagger.");
}

// Map controllers to endpoints
app.MapControllers();

// Set the port from Render or default to 8080
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}"); // Listen on the correct port

// Run the application
app.Run();
