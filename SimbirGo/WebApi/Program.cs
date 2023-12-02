using Application.Options;
using Application.Dtos.Mapping;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using WebApp.Middleware;
using Application.Extensions;
using System.Text.Json.Serialization;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

AddDbContext(builder);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services
    .AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

ConfigureJwtAuthentication(builder);
builder.Services.AddAuthorization();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient(s => s.GetRequiredService<IHttpContextAccessor>()!.HttpContext!.User);
builder.Services.AddServices();
builder.Services.AddExceptionHandlingMiddleware();
builder.Services.AddSessionValidationMiddleware();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(ConfigureSwaggerAuthentication);

var app = builder.Build();

await MigrateDbAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseExceptionHandlingMiddleware();
app.UseSessionJwtValidationMiddleware();

app.MapControllers();

app.Run();

static void AddDbContext(WebApplicationBuilder builder)
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseNpgsql(connectionString));
}

static async Task MigrateDbAsync(WebApplication app)
{
    using IServiceScope serviceScope = app.Services.CreateScope();
    using ApplicationDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

static void ConfigureSwaggerAuthentication(SwaggerGenOptions options)
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
}

static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
{
    builder.Services.Configure<JwtOptions>(
        builder.Configuration.GetRequiredSection("JwtSettings"));
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SigningKey"]!))
        });
}
