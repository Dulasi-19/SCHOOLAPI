using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Microsoft.Extensions.Options;
using schoolmanagement.Api;
using schoolmanagement.Api.Auth;
using schoolmanagement.Api.DI;
using schoolmanagement.Api.Middleware;
using Asp.Versioning.ApiExplorer;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var authSettings = builder.Configuration.GetSection("Auth").Get<AuthSettings>() ?? new AuthSettings();

    // 1. Register Application Services, Repositories, AutoMapper & CORS
    builder.Services.RegisteredServices(builder.Configuration);
    builder.Services.RegisterCorsPolicy(builder.Configuration);
    builder.Services.AddSchoolManagementAuth(Options.Create(authSettings));

    // 2. Read Connection String directly from appsettings.json
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    var serverVersion = new MySqlServerVersion(new Version(8, 0, 36));
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString, serverVersion));

    builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<AppDbContext>());

    var app = builder.Build();

    // 3. Exception Handling Middleware
    app.UseMiddleware<ExceptionMiddleware>();

    // 4. Configure HTTP Request Pipeline & Swagger UI with API Versioning
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty; // Serves Swagger UI at root URL (e.g. http://localhost:5062/)
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
        }
    });

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy_UISettings");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application startup failed: {ex.Message}");
    throw;
}
