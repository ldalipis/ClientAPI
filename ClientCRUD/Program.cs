using ClientCRUD.Configurations;
using ClientCRUD.Endpoints;
using ClientCRUD.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register services
var loaderSettings = builder.Configuration.GetSection("LoaderSettings").Get<LoaderSettings>();
if (loaderSettings == null)
{
    throw new InvalidOperationException("Invalid LoaderSettings configuration.");
}

var validationResults = new LoaderSettingsValidation().Validate("LoaderSettings", loaderSettings);
if (validationResults.Failed)
{
    throw new InvalidOperationException("LoaderSettings validation failed: " + validationResults.FailureMessage);
}

builder.Services.AddLoaderServices(loaderSettings);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Client CRUD API",
        Description = "An API to perform CRUD operations using FileLoader or SqlServerLoader."
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder => builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Enable Swagger UI in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client CRUD API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors("AllowLocalhost4200");
app.UseHttpsRedirection();
app.UseExceptionHandler("/error");

app.MapEndpoints();

app.Run();
