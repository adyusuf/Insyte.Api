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

    // --- Çalışma Grupları ---
    var wg1Id = Guid.NewGuid();
    var existingWg1 = await context.WorkingGroups.FirstOrDefaultAsync(w => w.Name == "Matematik Zümresi" && w.SchoolId == schoolId);

    if (existingWg1 == null)
    {
        var wg1 = new WorkingGroup
        {
            Id = wg1Id,
            Name = "Matematik Zümresi",
            Description = "Matematik derslerinde yenilikçi öğretim yöntemleri geliştiren çalışma grubu",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.WorkingGroups.Add(wg1);
        Console.WriteLine("✓ Çalışma grubu oluşturuldu: Matematik Zümresi");
    }
    else
    {
        wg1Id = existingWg1.Id;
    }

    var wg2Id = Guid.NewGuid();
    var existingWg2 = await context.WorkingGroups.FirstOrDefaultAsync(w => w.Name == "Teknoloji Entegrasyon Grubu" && w.SchoolId == schoolId);

    if (existingWg2 == null)
    {
        var wg2 = new WorkingGroup
        {
            Id = wg2Id,
            Name = "Teknoloji Entegrasyon Grubu",
            Description = "Dijital araçların sınıf ortamına entegrasyonunu planlayan grup",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.WorkingGroups.Add(wg2);
        Console.WriteLine("✓ Çalışma grubu oluşturuldu: Teknoloji Entegrasyon Grubu");
    }
    else
    {
        wg2Id = existingWg2.Id;
    }

    var wg3Id = Guid.NewGuid();
    var existingWg3 = await context.WorkingGroups.FirstOrDefaultAsync(w => w.Name == "Özel Eğitim Destek Grubu" && w.SchoolId == schoolId);

    if (existingWg3 == null)
    {
        var wg3 = new WorkingGroup
        {
            Id = wg3Id,
            Name = "Özel Eğitim Destek Grubu",
            Description = "Özel gereksinimli öğrencilere yönelik destek stratejileri geliştiren grup",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.WorkingGroups.Add(wg3);
        Console.WriteLine("✓ Çalışma grubu oluşturuldu: Özel Eğitim Destek Grubu");
    }
    else
    {
        wg3Id = existingWg3.Id;
    }

    await context.SaveChangesAsync();

    // --- Çalışma Grubu Üyeleri ---
    var wgMember1Exists = await context.WorkingGroupMembers.AnyAsync(m => m.WorkingGroupId == wg1Id && m.UserId == teacher1Id);
    if (!wgMember1Exists)
    {
        context.WorkingGroupMembers.Add(new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = wg1Id,
            UserId = teacher1Id,
            Role = "Başkan",
            AssignedAt = DateTime.UtcNow
        });
    }

    var wgMember2Exists = await context.WorkingGroupMembers.AnyAsync(m => m.WorkingGroupId == wg1Id && m.UserId == teacher2Id);
    if (!wgMember2Exists)
    {
        context.WorkingGroupMembers.Add(new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = wg1Id,
            UserId = teacher2Id,
            Role = "Üye",
            AssignedAt = DateTime.UtcNow
        });
    }

    var wgMember3Exists = await context.WorkingGroupMembers.AnyAsync(m => m.WorkingGroupId == wg2Id && m.UserId == schoolAdminId);
    if (!wgMember3Exists)
    {
        context.WorkingGroupMembers.Add(new WorkingGroupMember
        {
            Id = Guid.NewGuid(),
            WorkingGroupId = wg2Id,
            UserId = schoolAdminId,
            Role = "Başkan",
            AssignedAt = DateTime.UtcNow
        });
    }

    await context.SaveChangesAsync();
    Console.WriteLine("✓ Çalışma grubu üyeleri eklendi");

    // --- Kurullar ---
    var c1Id = Guid.NewGuid();
    var existingC1 = await context.Councils.FirstOrDefaultAsync(c => c.Name == "Okul Yönetim Kurulu" && c.SchoolId == schoolId);

    if (existingC1 == null)
    {
        var council1 = new Council
        {
            Id = c1Id,
            Name = "Okul Yönetim Kurulu",
            Description = "Okul genelinde stratejik kararlar alan üst kurul",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Councils.Add(council1);
        Console.WriteLine("✓ Kurul oluşturuldu: Okul Yönetim Kurulu");
    }
    else
    {
        c1Id = existingC1.Id;
    }

    var c2Id = Guid.NewGuid();
    var existingC2 = await context.Councils.FirstOrDefaultAsync(c => c.Name == "Disiplin Kurulu" && c.SchoolId == schoolId);

    if (existingC2 == null)
    {
        var council2 = new Council
        {
            Id = c2Id,
            Name = "Disiplin Kurulu",
            Description = "Öğrenci disiplin süreçlerini yürüten kurul",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Councils.Add(council2);
        Console.WriteLine("✓ Kurul oluşturuldu: Disiplin Kurulu");
    }
    else
    {
        c2Id = existingC2.Id;
    }

    var c3Id = Guid.NewGuid();
    var existingC3 = await context.Councils.FirstOrDefaultAsync(c => c.Name == "Rehberlik ve Psikolojik Danışma Kurulu" && c.SchoolId == schoolId);

    if (existingC3 == null)
    {
        var council3 = new Council
        {
            Id = c3Id,
            Name = "Rehberlik ve Psikolojik Danışma Kurulu",
            Description = "Öğrenci rehberlik hizmetlerini koordine eden kurul",
            SchoolId = schoolId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Councils.Add(council3);
        Console.WriteLine("✓ Kurul oluşturuldu: Rehberlik ve Psikolojik Danışma Kurulu");
    }
    else
    {
        c3Id = existingC3.Id;
    }

    await context.SaveChangesAsync();

    // --- Kurul Üyeleri ---
    var cMember1Exists = await context.CouncilMembers.AnyAsync(m => m.CouncilId == c1Id && m.UserId == schoolAdminId);
    if (!cMember1Exists)
    {
        context.CouncilMembers.Add(new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = c1Id,
            UserId = schoolAdminId,
            Role = "Başkan",
            AssignedAt = DateTime.UtcNow
        });
    }

    var cMember2Exists = await context.CouncilMembers.AnyAsync(m => m.CouncilId == c1Id && m.UserId == teacher1Id);
    if (!cMember2Exists)
    {
        context.CouncilMembers.Add(new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = c1Id,
            UserId = teacher1Id,
            Role = "Üye",
            AssignedAt = DateTime.UtcNow
        });
    }

    var cMember3Exists = await context.CouncilMembers.AnyAsync(m => m.CouncilId == c2Id && m.UserId == teacher2Id);
    if (!cMember3Exists)
    {
        context.CouncilMembers.Add(new CouncilMember
        {
            Id = Guid.NewGuid(),
            CouncilId = c2Id,
            UserId = teacher2Id,
            Role = "Raportör",
            AssignedAt = DateTime.UtcNow
        });
    }

    await context.SaveChangesAsync();
    Console.WriteLine("✓ Kurul üyeleri eklendi");

    Console.WriteLine("\n=== Seed Tamamlandı ===");
    Console.WriteLine("\nKullanıcı Bilgileri:");
    Console.WriteLine("  Admin:          admin@insyte.com / Admin@123");
    Console.WriteLine("  Danışman:       danisman@insyte.com / Danisman@123");
    Console.WriteLine("  Okul Yöneticisi: yonetici@anadolu.k12.tr / Yonetici@123");
    Console.WriteLine("  Öğretmen 1:     ogretmen1@anadolu.k12.tr / Ogretmen@123");
    Console.WriteLine("  Öğretmen 2:     ogretmen2@anadolu.k12.tr / Ogretmen@123");
}
