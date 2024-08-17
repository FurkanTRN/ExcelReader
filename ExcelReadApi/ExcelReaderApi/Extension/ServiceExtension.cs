using System.Text;
using ExcelReadApi.Context;
using ExcelReadApi.Interface;
using ExcelReadApi.Repository;
using ExcelReadApi.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;


namespace ExcelReadApi.Extension;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AddLogging();
        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()));
    }

    public static void AddDbContext(this IServiceCollection services) =>
        services.AddDbContext<ExcelReaderApiDbContext>(ServiceLifetime.Transient);

    public static void AddScoped(this IServiceCollection services)
    {
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IPrintHistoryRepository,PrintHistoryRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IExcelProcessingService, ExcelProcessingService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IPrintHistoryService, PrintHistoryService>();
    }

    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "ExcelReader API",
                Version = "v1",
                Description = "API for Excel Files"
            });

            var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}