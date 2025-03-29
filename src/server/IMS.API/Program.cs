
using IMS.API;
using IMS.API.Middlewares;
using IMS.Data;
using IMS.Data.SeedData;
using IMS.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterServicesAndMediatR(builder.Configuration);

builder.Services.RegisterAuthentication(builder.Configuration);

builder.Services.RegisterSwagger(builder.Configuration);

builder.Services.RegisterVersioning(builder.Configuration);

builder.Services.RegisterCors(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<IMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<IMSDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

    var rolesJsonPath = Path.Combine(app.Environment.WebRootPath, "data", "role.json");
    var usersJsonPath = Path.Combine(app.Environment.WebRootPath, "data", "user.json");

    Console.WriteLine($"WebRootPath: {app.Environment.WebRootPath}");
    Console.WriteLine($"Roles JSON path: {rolesJsonPath}");
    Console.WriteLine($"Users JSON path: {usersJsonPath}");
    Console.WriteLine($"Roles file exists: {File.Exists(rolesJsonPath)}");
    Console.WriteLine($"Users file exists: {File.Exists(usersJsonPath)}");

    // Ensure the directory exists
    Directory.CreateDirectory(Path.Combine(app.Environment.WebRootPath, "data"));

    try
    {
        await DbInitializer.Initialize(context, roleManager, userManager,rolesJsonPath, usersJsonPath);
        Console.WriteLine("Database seeded successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding database: {ex.Message}");
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "IMS Web API v1");
        options.EnableDeepLinking();
        options.DisplayRequestDuration();
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
    app.MapOpenApi();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCustomExceptionHandler();
app.UseCors("CorsPolicy");
await app.RunAsync();

