using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeoplePortal.Application;
using PeoplePortal.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "PeoplePortal API", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insert your Bearer token.",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var keycloakAuthority = builder.Configuration["Authentication:Authority"]
    ?? throw new InvalidOperationException("Authentication:Authority is required.");

var keycloakAudience = builder.Configuration["Authentication:Audience"]
    ?? throw new InvalidOperationException("Authentication:Audience is required.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakAuthority;
        options.Audience = keycloakAudience;
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = keycloakAudience,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = "preferred_username",
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                if (context.Principal?.Identity is not ClaimsIdentity identity)
                {
                    return Task.CompletedTask;
                }

                var realmAccess = context.Principal.FindFirst("realm_access")?.Value;
                if (string.IsNullOrWhiteSpace(realmAccess))
                {
                    return Task.CompletedTask;
                }

                using JsonDocument document = JsonDocument.Parse(realmAccess);
                if (!document.RootElement.TryGetProperty("roles", out JsonElement rolesElement) ||
                    rolesElement.ValueKind != JsonValueKind.Array)
                {
                    return Task.CompletedTask;
                }

                var existingRoles = new HashSet<string>(
                    identity.FindAll(ClaimTypes.Role).Select(c => c.Value));

                foreach (var role in rolesElement.EnumerateArray())
                {
                    var roleValue = role.GetString();
                    if (!string.IsNullOrWhiteSpace(roleValue) && existingRoles.Add(roleValue))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, roleValue));
                    }
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeePolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("employee"));

    options.AddPolicy("ManagerPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("jefe_inmediato"));

    options.AddPolicy("HrPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("hr", "admin"));

    options.AddPolicy("NominaPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("nomina"));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireAuthenticatedUser().RequireRole("admin"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:5173", "http://localhost:5174", "http://localhost:30081", "http://localhost:30082")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<PeoplePortal.Api.Middleware.ExceptionHandlingMiddleware>();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
