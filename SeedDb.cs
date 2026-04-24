using System;
using System.Threading.Tasks;
using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Insyte.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<InsyteDbContext>()
    .UseNpgsql("Host=localhost;Port=5432;Database=insyte;Username=postgres;Password=123456")
    .Options;

using (var context = new InsyteDbContext(options))
{
    // Check if admin user already exists
    var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@insyte.com");
    
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

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
        
        Console.WriteLine("✓ Admin user created successfully");
        Console.WriteLine($"Email: admin@insyte.com");
        Console.WriteLine($"Password: Admin@123");
    }
    else
    {
        Console.WriteLine("✓ Admin user already exists");
    }
}
