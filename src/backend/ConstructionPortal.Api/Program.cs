using System.Text;
using ConstructionPortal.Api.Data;
using ConstructionPortal.Api.Endpoints;
using ConstructionPortal.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<JwtService>();
builder.Services.AddScoped<ErpSyncService>();
builder.Services.AddScoped<AlertRulesService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var jwtKey = builder.Configuration["Jwt:Key"] ?? "";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("ui", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseCors("ui");
app.UseAuthentication();
app.UseAuthorization();

// Ensure DB exists + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seedEnabled = builder.Configuration.GetValue<bool>("Seed:Enable");

    if (seedEnabled)
    {
        await DbSeeder.SeedAsync(db);
    }
    else
    {
        await db.Database.MigrateAsync();
    }
}

app.MapGet("/health", () => Results.Ok(new { Status = "OK" }));

app.MapAuth();
app.MapProjects();
app.MapReports();
app.MapCosts();
app.MapAlerts();

app.Run();
