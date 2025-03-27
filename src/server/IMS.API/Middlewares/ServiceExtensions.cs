using System;
using System.Text;
using IMS.API.ConfigurationOptions;
using IMS.Business.Handlers;
using IMS.Business.Handlers.Auth;
using IMS.Business.Services;
using IMS.Data;
using IMS.Data.Repositories;
using IMS.Data.UnitOfWorks;
using IMS.Models.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace IMS.API;

public static class ServiceExtensions
{

    public static IServiceCollection RegisterAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Identity: UserManager, RoleManager, SignInManager
        services.AddIdentity<User, Role>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<IMSDbContext>()
            .AddDefaultTokenProviders();

        // Register JWT with Bearer token
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    configuration["JWT:Secret"] ?? "supersecuredsecretkey"))
            };
        });
        return services;
    }

    public static IServiceCollection RegisterServicesAndMediatR(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Services and MediatR
        // Register MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(LoginRequestCommand).Assembly));

        // Resgister MediatoR for Candidate
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(CandidateGetAllQuery).Assembly));
        // Register UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Register IUserIdentity to get current user
        services.AddScoped<IUserIdentity, UserIdentity>();
        // Register Services
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IPasswordService, PasswordService>();


        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // Register SMTP token lifespan
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(configuration.GetValue<int>("SmtpSettings:ResetPasswordTokenExpiryMinutes"));
        });
        return services;
    }

    public static IServiceCollection RegisterSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "ViVuStore Web API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    new string[] { }
                                }
                            });
            });

        return services;
    }


    public static IServiceCollection RegisterVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Versioning
        services.AddVersionedApiExplorer(options =>
        {
            // Add version 1.0 to the explorer
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });
        return services;
    }

    public static IServiceCollection RegisterCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", opt => opt
                .WithOrigins(configuration.GetSection("CORs:AllowedOrigins").Get<string[]>() ?? [])
                .WithHeaders(configuration.GetSection("CORs:AllowedHeaders").Get<string[]>() ?? [])
                .WithMethods(configuration.GetSection("CORs:AllowedMethods").Get<string[]>() ?? []));

            options.AddPolicy("AllowAnyOrigin", opt => opt
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
        return services;
    }


}
