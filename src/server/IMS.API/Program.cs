
using IMS.API;
using IMS.API.Middlewares;
using IMS.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.RegisterServicesAndMediatR(builder.Configuration);

builder.Services.RegisterAuthentication(builder.Configuration);

builder.Services.RegisterSwagger(builder.Configuration);

builder.Services.RegisterVersioning(builder.Configuration);

builder.Services.RegisterCors(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddDbContext<IMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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

