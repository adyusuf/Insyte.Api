using System.Text;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<InsyteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("AdminOrAdvisor", policy => policy.RequireRole("Admin", "Advisor"));
    options.AddPolicy("AllStaff", policy => policy.RequireRole("Admin", "Advisor", "SchoolAdmin"));
});

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:5173",
            "http://localhost:5174",
            "http://localhost:5175",
            "http://localhost:5176",
            "http://localhost:5177",
            "http://localhost:5178",
            "http://localhost:5179",
            "http://localhost:5180",
            "http://localhost:8081",
            "http://localhost:8082",
            "http://127.0.0.1:8081",
            "http://127.0.0.1:8082"
        )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

// Auto-migrate on startup (development)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InsyteDbContext>();
    db.Database.Migrate();

    // Ensure admin user exists with correct password
    var existingAdmin = db.Users.FirstOrDefault(u => u.Email == "admin@insyte.com");
    if (existingAdmin == null)
    {
        var adminUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@insyte.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            FirstName = "Admin",
            LastName = "User",
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        db.Users.Add(adminUser);
        db.SaveChanges();

        Console.WriteLine("✓ Admin user seeded: admin@insyte.com / Admin@123");
    }
    else
    {
        // Update password hash if needed
        var correctHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
        if (existingAdmin.PasswordHash != correctHash)
        {
            existingAdmin.PasswordHash = correctHash;
            existingAdmin.UpdatedAt = DateTime.UtcNow;
            db.SaveChanges();
            Console.WriteLine("✓ Admin password updated");
        }
        Console.WriteLine("✓ Admin user exists: admin@insyte.com / Admin@123");
    }
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
