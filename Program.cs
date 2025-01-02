using MongoExample.Models;
using MongoExample.Services;
using Microsoft.OpenApi.Models;

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
        Description = "A RESTful API for for herbs. Search for a Herb and check the pharmaceutical data. This data is from European Medicines Agency",
        Contact = new OpenApiContact
        {
            Name = "Designed By Cyprain Chidozie",
            Email = "Cyprainchidozie232@gmail.com",
            Url = new Uri("https://cypso05.github.io/softwaredev/")
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PharmaHerbs API v1");
        options.DocumentTitle = "PharmaHerbs API Documentation Data from EMA"; // Title for the Swagger UI page
    });
}

// Enable routing
app.UseRouting();

// Map controllers to endpoints
app.MapControllers();

// Run the application
app.Run();
