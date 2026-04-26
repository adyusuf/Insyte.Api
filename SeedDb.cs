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
    Console.WriteLine("=== Insyte Seed Verisi Yükleniyor ===\n");

    // --- Admin Kullanıcı ---
    var adminId = Guid.NewGuid();
    var existingAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@insyte.com");

    if (existingAdmin == null)
    {
        var adminUser = new User
        {
            Id = adminId,
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
        Console.WriteLine("✓ Admin kullanıcısı oluşturuldu: admin@insyte.com / Admin@123");
    }
    else
    {
        adminId = existingAdmin.Id;
        Console.WriteLine("✓ Admin kullanıcısı mevcut");
    }

    // --- Danışman Kullanıcı ---
    var advisorId = Guid.NewGuid();
    var existingAdvisor = await context.Users.FirstOrDefaultAsync(u => u.Email == "danisman@insyte.com");

    if (existingAdvisor == null)
    {
        var advisorUser = new User
        {
            Id = advisorId,
            Email = "danisman@insyte.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Danisman@123"),
            FirstName = "Mehmet",
            LastName = "Yılmaz",
            Role = UserRole.Advisor,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(advisorUser);
        await context.SaveChangesAsync();
        Console.WriteLine("✓ Danışman kullanıcısı oluşturuldu: danisman@insyte.com / Danisman@123");
    }
    else
    {
        advisorId = existingAdvisor.Id;
        Console.WriteLine("✓ Danışman kullanıcısı mevcut");
    }

    // --- Okul Yöneticisi ---
    var schoolAdminId = Guid.NewGuid();
    var existingSchoolAdmin = await context.Users.FirstOrDefaultAsync(u => u.Email == "yonetici@anadolu.k12.tr");

    if (existingSchoolAdmin == null)
    {
        var schoolAdminUser = new User
        {
            Id = schoolAdminId,
            Email = "yonetici@anadolu.k12.tr",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Yonetici@123"),
            FirstName = "Ayşe",
            LastName = "Kaya",
            Role = UserRole.SchoolAdmin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(schoolAdminUser);
        await context.SaveChangesAsync();
        Console.WriteLine("✓ Okul yöneticisi oluşturuldu: yonetici@anadolu.k12.tr / Yonetici@123");
    }
    else
    {
        schoolAdminId = existingSchoolAdmin.Id;
        Console.WriteLine("✓ Okul yöneticisi mevcut");
    }

    // --- Öğretmen Kullanıcıları ---
    var teacher1Id = Guid.NewGuid();
    var existingTeacher1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "ogretmen1@anadolu.k12.tr");

    if (existingTeacher1 == null)
    {
        var teacher1 = new User
        {
            Id = teacher1Id,
            Email = "ogretmen1@anadolu.k12.tr",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ogretmen@123"),
            FirstName = "Ali",
            LastName = "Demir",
            Role = UserRole.Teacher,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(teacher1);
        Console.WriteLine("✓ Öğretmen 1 oluşturuldu: Ali Demir");
    }
    else
    {
        teacher1Id = existingTeacher1.Id;
    }

    var teacher2Id = Guid.NewGuid();
    var existingTeacher2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "ogretmen2@anadolu.k12.tr");

    if (existingTeacher2 == null)
    {
        var teacher2 = new User
        {
            Id = teacher2Id,
            Email = "ogretmen2@anadolu.k12.tr",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ogretmen@123"),
            FirstName = "Fatma",
            LastName = "Şahin",
            Role = UserRole.Teacher,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(teacher2);
        Console.WriteLine("✓ Öğretmen 2 oluşturuldu: Fatma Şahin");
    }
    else
    {
        teacher2Id = existingTeacher2.Id;
    }

    await context.SaveChangesAsync();

    // --- Okul ---
    var schoolId = Guid.NewGuid();
    var existingSchool = await context.Schools.FirstOrDefaultAsync(s => s.Name == "Anadolu İlköğretim Okulu");

    if (existingSchool == null)
    {
        var school = new School
        {
            Id = schoolId,
            Name = "Anadolu İlköğretim Okulu",
            Address = "Cumhuriyet Cad. No:42",
            City = "Ankara",
            Phone = "+90 312 555 01 23",
            Email = "iletisim@anadolu.k12.tr",
            SchoolType = SchoolType.Ilkokul,
            InstitutionType = InstitutionType.Devlet,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Schools.Add(school);
        await context.SaveChangesAsync();
        Console.WriteLine("✓ Okul oluşturuldu: Anadolu İlköğretim Okulu");
    }
    else
    {
        schoolId = existingSchool.Id;
        Console.WriteLine("✓ Okul mevcut");
    }

    await context.SaveChangesAsync();

    Console.WriteLine("\n=== Seed Tamamlandı ===");
    Console.WriteLine("\nKullanıcı Bilgileri:");
    Console.WriteLine("  Admin:          admin@insyte.com / Admin@123");
    Console.WriteLine("  Danışman:       danisman@insyte.com / Danisman@123");
    Console.WriteLine("  Okul Yöneticisi: yonetici@anadolu.k12.tr / Yonetici@123");
    Console.WriteLine("  Öğretmen 1:     ogretmen1@anadolu.k12.tr / Ogretmen@123");
    Console.WriteLine("  Öğretmen 2:     ogretmen2@anadolu.k12.tr / Ogretmen@123");
}
