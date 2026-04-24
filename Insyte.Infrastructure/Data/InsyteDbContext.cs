using Insyte.Core.Entities;
using Insyte.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace Insyte.Infrastructure.Data;

public class InsyteDbContext : DbContext
{
    public InsyteDbContext(DbContextOptions<InsyteDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w =>
            w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<School> Schools => Set<School>();
    public DbSet<SchoolAdvisor> SchoolAdvisors => Set<SchoolAdvisor>();
    public DbSet<SchoolUser> SchoolUsers => Set<SchoolUser>();
    public DbSet<AIProvider> AIProviders => Set<AIProvider>();
    public DbSet<AIModel> AIModels => Set<AIModel>();
    public DbSet<EvaluationCriteria> EvaluationCriteria => Set<EvaluationCriteria>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<Evaluation> Evaluations => Set<Evaluation>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<EmailConfig> EmailConfigs => Set<EmailConfig>();
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<SchoolFacility> SchoolFacilities => Set<SchoolFacility>();
    public DbSet<SchoolServiceOffering> SchoolServices => Set<SchoolServiceOffering>();
    public DbSet<SchoolActivity> SchoolActivities => Set<SchoolActivity>();
    public DbSet<SchoolLanguage> SchoolLanguages => Set<SchoolLanguage>();
    public DbSet<EvaluationQuestion> EvaluationQuestions => Set<EvaluationQuestion>();
    public DbSet<WorkingGroup> WorkingGroups => Set<WorkingGroup>();
    public DbSet<WorkingGroupMember> WorkingGroupMembers => Set<WorkingGroupMember>();
    public DbSet<Council> Councils => Set<Council>();
    public DbSet<CouncilMember> CouncilMembers => Set<CouncilMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasConversion<string>();
        });

        // School
        modelBuilder.Entity<School>(e =>
        {
            e.HasIndex(s => s.Name);
            e.Property(s => s.SchoolType).HasConversion<string>();
            e.Property(s => s.InstitutionType).HasConversion<string>();
            e.Property(s => s.LiseType).HasConversion<string>();
            e.Property(s => s.EducationSystem).HasConversion<string>();
        });

        // SchoolAdvisor
        modelBuilder.Entity<SchoolAdvisor>(e =>
        {
            e.HasIndex(sa => new { sa.SchoolId, sa.UserId }).IsUnique();
            e.HasOne(sa => sa.School).WithMany(s => s.Advisors).HasForeignKey(sa => sa.SchoolId);
            e.HasOne(sa => sa.User).WithMany(u => u.AdvisedSchools).HasForeignKey(sa => sa.UserId);
        });

        // SchoolUser
        modelBuilder.Entity<SchoolUser>(e =>
        {
            e.HasIndex(su => new { su.SchoolId, su.UserId }).IsUnique();
            e.Property(su => su.Role).HasConversion<string>();
            e.HasOne(su => su.School).WithMany(s => s.Users).HasForeignKey(su => su.SchoolId);
            e.HasOne(su => su.User).WithMany(u => u.SchoolUsers).HasForeignKey(su => su.UserId);
        });

        // AIProvider
        modelBuilder.Entity<AIProvider>(e =>
        {
            e.HasIndex(p => p.Name).IsUnique();
        });

        // AIModel
        modelBuilder.Entity<AIModel>(e =>
        {
            e.HasOne(m => m.AIProvider).WithMany(p => p.Models).HasForeignKey(m => m.AIProviderId);
        });

        // Video
        modelBuilder.Entity<Video>(e =>
        {
            e.Property(v => v.Status).HasConversion<string>();
            e.HasOne(v => v.School).WithMany(s => s.Videos).HasForeignKey(v => v.SchoolId);
            e.HasOne(v => v.Teacher).WithMany().HasForeignKey(v => v.TeacherUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(v => v.UploadedBy).WithMany(u => u.UploadedVideos).HasForeignKey(v => v.UploadedByUserId).OnDelete(DeleteBehavior.Restrict);
        });

        // Evaluation
        modelBuilder.Entity<Evaluation>(e =>
        {
            e.Property(ev => ev.Status).HasConversion<string>();
            e.HasOne(ev => ev.Video).WithMany(v => v.Evaluations).HasForeignKey(ev => ev.VideoId);
            e.HasOne(ev => ev.Criteria).WithMany(c => c.Evaluations).HasForeignKey(ev => ev.CriteriaId);
            e.HasOne(ev => ev.AIModel).WithMany(m => m.Evaluations).HasForeignKey(ev => ev.AIModelId);
        });

        // Report
        modelBuilder.Entity<Report>(e =>
        {
            e.Property(r => r.Status).HasConversion<string>();
            e.HasOne(r => r.Evaluation).WithOne(ev => ev.Report).HasForeignKey<Report>(r => r.EvaluationId);
            e.HasOne(r => r.ApprovedBy).WithMany().HasForeignKey(r => r.ApprovedByUserId).OnDelete(DeleteBehavior.SetNull);
        });

        // EmailConfig
        modelBuilder.Entity<EmailConfig>(e =>
        {
            e.Property(ec => ec.RecipientType).HasConversion<string>();
            e.HasOne(ec => ec.School).WithMany(s => s.EmailConfigs).HasForeignKey(ec => ec.SchoolId);
        });

        // EmailLog
        modelBuilder.Entity<EmailLog>(e =>
        {
            e.Property(el => el.Status).HasConversion<string>();
            e.HasOne(el => el.Report).WithMany(r => r.EmailLogs).HasForeignKey(el => el.ReportId);
        });

        // Class
        modelBuilder.Entity<Class>(e =>
        {
            e.Property(c => c.Level).HasConversion<string>();
            e.HasOne(c => c.School).WithMany(s => s.Classes).HasForeignKey(c => c.SchoolId);
            e.HasIndex(c => new { c.SchoolId, c.Name });
        });

        // Subject
        modelBuilder.Entity<Subject>(e =>
        {
            e.HasOne(s => s.School).WithMany(sch => sch.Subjects).HasForeignKey(s => s.SchoolId);
            e.HasIndex(s => new { s.SchoolId, s.Name });
        });

        // Schedule
        modelBuilder.Entity<Schedule>(e =>
        {
            e.Property(sch => sch.DayOfWeek).HasConversion<string>();
            e.HasOne(sch => sch.School).WithMany(s => s.Schedules).HasForeignKey(sch => sch.SchoolId);
            e.HasOne(sch => sch.Class).WithMany(c => c.Schedules).HasForeignKey(sch => sch.ClassId);
            e.HasOne(sch => sch.Subject).WithMany(s => s.Schedules).HasForeignKey(sch => sch.SubjectId);
            e.HasOne(sch => sch.Teacher).WithMany().HasForeignKey(sch => sch.TeacherUserId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(sch => new { sch.ClassId, sch.DayOfWeek, sch.StartTime });
        });

        // SchoolFacility
        modelBuilder.Entity<SchoolFacility>(e =>
        {
            e.Property(sf => sf.Facility).HasConversion<string>();
            e.HasOne(sf => sf.School).WithMany(s => s.Facilities).HasForeignKey(sf => sf.SchoolId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(sf => new { sf.SchoolId, sf.Facility }).IsUnique();
        });

        // SchoolServiceOffering
        modelBuilder.Entity<SchoolServiceOffering>(e =>
        {
            e.Property(ss => ss.Service).HasConversion<string>();
            e.HasOne(ss => ss.School).WithMany(s => s.Services).HasForeignKey(ss => ss.SchoolId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(ss => new { ss.SchoolId, ss.Service }).IsUnique();
        });

        // SchoolActivity
        modelBuilder.Entity<SchoolActivity>(e =>
        {
            e.Property(sa => sa.Activity).HasConversion<string>();
            e.HasOne(sa => sa.School).WithMany(s => s.Activities).HasForeignKey(sa => sa.SchoolId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(sa => new { sa.SchoolId, sa.Activity }).IsUnique();
        });

        // SchoolLanguage
        modelBuilder.Entity<SchoolLanguage>(e =>
        {
            e.Property(sl => sl.Language).HasConversion<string>();
            e.HasOne(sl => sl.School).WithMany(s => s.Languages).HasForeignKey(sl => sl.SchoolId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(sl => new { sl.SchoolId, sl.Language }).IsUnique();
        });

        // EvaluationQuestion
        modelBuilder.Entity<EvaluationQuestion>(e =>
        {
            e.HasOne(q => q.Criteria).WithMany(c => c.Questions).HasForeignKey(q => q.CriteriaId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(q => new { q.CriteriaId, q.Order });
        });

        // WorkingGroup
        modelBuilder.Entity<WorkingGroup>(e =>
        {
            e.HasOne(wg => wg.School).WithMany(s => s.WorkingGroups).HasForeignKey(wg => wg.SchoolId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(wg => new { wg.SchoolId, wg.Name });
        });

        // WorkingGroupMember
        modelBuilder.Entity<WorkingGroupMember>(e =>
        {
            e.HasIndex(wgm => new { wgm.WorkingGroupId, wgm.UserId }).IsUnique();
            e.HasOne(wgm => wgm.WorkingGroup).WithMany(wg => wg.Members).HasForeignKey(wgm => wgm.WorkingGroupId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(wgm => wgm.User).WithMany(u => u.WorkingGroupMemberships).HasForeignKey(wgm => wgm.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // Council
        modelBuilder.Entity<Council>(e =>
        {
            e.HasOne(c => c.School).WithMany(s => s.Councils).HasForeignKey(c => c.SchoolId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(c => new { c.SchoolId, c.Name });
        });

        // CouncilMember
        modelBuilder.Entity<CouncilMember>(e =>
        {
            e.HasIndex(cm => new { cm.CouncilId, cm.UserId }).IsUnique();
            e.HasOne(cm => cm.Council).WithMany(c => c.Members).HasForeignKey(cm => cm.CouncilId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(cm => cm.User).WithMany(u => u.CouncilMemberships).HasForeignKey(cm => cm.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // Seed admin user
        var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            Email = "admin@insyte.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            FirstName = "System",
            LastName = "Admin",
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });

        // Seed evaluation criteria - with IDs for question references
        var criteria1 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), Name = "Pedagojik Yeterlilik", Description = "Dersin yapısı, hedef belirleme, müfredatla uyum, konuyu doğru aktarma", Instructions = "Öğretmenin ders planını oluşturma, hedefler belirleme ve müfredata uygun içerik sunma becerisini değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria2 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), Name = "İletişim & Sunum Becerileri", Description = "Ses tonu, konuşma hızı, beden dili, göz teması, dil kullanımı", Instructions = "Öğretmenin iletişim stilini, konuşma netliğini, beden dilini ve sınıf içinde etkileşim kurma becerilerini değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria3 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000003"), Name = "Öğrenci Katılımı & Motivasyonu", Description = "Öğrencileri derse dahil etme, sorular sorma, ilgiyi canlı tutma", Instructions = "Öğrencilerin derse katılım düzeyini, motivasyon seviyesini ve öğretmenin etkileşim kurma çabalarını değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria4 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000004"), Name = "Sınıf Yönetimi", Description = "Düzeni sağlama, zaman yönetimi, disiplin yaklaşımı", Instructions = "Sınıf düzeninin sağlanması, zamanın etkili kullanılması ve disiplin yaklaşımının uygunluğunu değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria5 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000005"), Name = "Soru-Cevap & Geri Bildirim Kalitesi", Description = "Öğrenci sorularını yanıtlama, anlık geri bildirim, yönlendirme", Instructions = "Öğrenci sorularına verilen cevapların kalitesini, geri bildirim etkinliğini ve yapıcı yönlendirmeyi değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria6 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000006"), Name = "Materyal & Görsel Kullanımı", Description = "Tahta, slayt, araç-gereç kullanımı, materyallerin etkinliği", Instructions = "Öğretim materyallerinin uygunluğunu, çeşitliliğini ve etkili kullanımını değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria7 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000007"), Name = "Farklılaştırılmış Öğretim", Description = "Farklı öğrenme düzeylerine hitap etme, bireysel ilgi", Instructions = "Öğretmenin farklı öğrenme stillerine ve seviyelere uygun öğretim stratejileri kullanıp kullanmadığını değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria8 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000008"), Name = "Öğrenci Davranışı & Tepkileri", Description = "Öğrencilerin ilgi düzeyi, katılım, beden dili, motivasyon", Instructions = "Öğrencilerin derste gösterdikleri ilgi, katılım düzeyi, beden dilini ve genel motivasyon durumunu değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria9 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000009"), Name = "Dil & Kavram Doğruluğu", Description = "Konuyu doğru ve anlaşılır aktarma, yanlış bilgi var mı", Instructions = "Öğretmenin konuyu doğru bir şekilde anlatan, yanlış bilgi içermeyen ve anlaşılır bir dil kullanıp kullanmadığını değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };
        var criteria10 = new EvaluationCriteria { Id = Guid.Parse("10000000-0000-0000-0000-000000000010"), Name = "Genel Ders Akışı & Tempo", Description = "Başlangıç-orta-kapanış dengesi, geçişler, süre yönetimi", Instructions = "Dersin başlangıcından sonuna kadar akışını, geçişlerin uygunluğunu ve zamanın dengeli kullanılıp kullanılmadığını değerlendirin.", IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) };

        modelBuilder.Entity<EvaluationCriteria>().HasData(criteria1, criteria2, criteria3, criteria4, criteria5, criteria6, criteria7, criteria8, criteria9, criteria10);

        // Seed evaluation questions
        var questions = new List<EvaluationQuestion>
        {
            // İletişim & Ses (7 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Öğretmenin sesi net ve anlaşılır mı, yoksa çok alçak/yüksek mi konuşuyor?", Category = "🗣️ İletişim & Ses", Order = 1, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Konuşma hızı uygun mu — çok hızlı veya çok yavaş mı?", Category = "🗣️ İletişim & Ses", Order = 2, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Öğretmen monoton mu konuşuyor, yoksa ses tonunda çeşitlilik var mı?", Category = "🗣️ İletişim & Ses", Order = 3, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Önemli kavramları vurgularken ses tonunu değiştiriyor mu?", Category = "🗣️ İletişim & Ses", Order = 4, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Türkçeyi (ya da ders dilini) doğru ve akıcı kullanıyor mu?", Category = "🗣️ İletişim & Ses", Order = 5, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Gereksiz dolgu sözler (yani, şey, hmm) ne sıklıkla kullanılıyor?", Category = "🗣️ İletişim & Ses", Order = 6, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Öğrencilere hitap ederken ne kadar samimi ve sıcak bir ton kullanıyor?", Category = "🗣️ İletişim & Ses", Order = 7, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Beden Dili & Fiziksel Varlık (5 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Öğretmen sınıfı aktif olarak dolaşıyor mu, yoksa tek noktada mı duruyor?", Category = "🧍 Beden Dili & Fiziksel Varlık", Order = 8, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Göz teması kuruyor mu — sadece tahtaya mı bakıyor, öğrencilere mi?", Category = "🧍 Beden Dili & Fiziksel Varlık", Order = 9, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "El, kol hareketleri anlatımı destekliyor mu?", Category = "🧍 Beden Dili & Fiziksel Varlık", Order = 10, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Öğrencilere karşı açık ve davetkar bir duruş mu sergiliyor?", Category = "🧍 Beden Dili & Fiziksel Varlık", Order = 11, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria2.Id, Question = "Yüz ifadeleri anlatılanlarla uyumlu mu?", Category = "🧍 Beden Dili & Fiziksel Varlık", Order = 12, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Pedagoji & Ders İçeriği (8 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Dersin başında öğrencilere neyi öğreneceklerini açıklıyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 13, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Konu anlatımı mantıksal bir sırayla ilerliyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 14, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Öğrettiği bilgiler doğru mu — kavram veya bilgi hatası var mı?", Category = "📚 Pedagoji & Ders İçeriği", Order = 15, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Konuyu günlük hayatla ilişkilendiriyor mu, somut örnekler veriyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 16, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Karmaşık kavramları basitten karmaşığa doğru açıklıyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 17, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Önceki derslerle bağlantı kuruyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 18, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Ders sonunda özet ya da tekrar yapıyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 19, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria1.Id, Question = "Ders hedefine ulaşıldı mı — dersin sonunda öğrenciler konuyu anlamış görünüyor mu?", Category = "📚 Pedagoji & Ders İçeriği", Order = 20, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Öğrenci Katılımı (8 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Öğrencilere ne sıklıkla soru soruyor?", Category = "🙋 Öğrenci Katılımı", Order = 21, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Soruları sadece belli öğrencilere mi, yoksa tüm sınıfa mı yöneltiyor?", Category = "🙋 Öğrenci Katılımı", Order = 22, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Öğrencilere düşünme zamanı (bekleme süresi) veriyor mu?", Category = "🙋 Öğrenci Katılımı", Order = 23, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Öğrenci cevaplarını dinliyor mu, yoksa cevabı kendisi mi tamamlıyor?", Category = "🙋 Öğrenci Katılımı", Order = 24, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Öğrencilerin birbirini dinlemesini ve tartışmasını teşvik ediyor mu?", Category = "🙋 Öğrenci Katılımı", Order = 25, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Gönüllü olmayan öğrencileri derse nasıl dahil ediyor?", Category = "🙋 Öğrenci Katılımı", Order = 26, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Grup çalışması ya da işbirlikli öğrenme var mı?", Category = "🙋 Öğrenci Katılımı", Order = 27, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria3.Id, Question = "Öğrencilerin kendi fikirlerini ifade etmelerine alan açıyor mu?", Category = "🙋 Öğrenci Katılımı", Order = 28, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Geri Bildirim & Soru-Cevap (6 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria5.Id, Question = "Öğrenci cevapları doğru olduğunda nasıl geri bildirim veriyor?", Category = "💬 Geri Bildirim & Soru-Cevap", Order = 29, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria5.Id, Question = "Yanlış cevaplarda öğrenciyi küçük düşürüyor mu, yoksa yapıcı mı yaklaşıyor?", Category = "💬 Geri Bildirim & Soru-Cevap", Order = 30, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria5.Id, Question = "Öğrencinin sorusunu doğru anlayıp yanıtlıyor mu?", Category = "💬 Geri Bildirim & Soru-Cevap", Order = 31, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria5.Id, Question = "Cevabı bilmeyen öğrenciye nasıl yaklaşıyor?", Category = "💬 Geri Bildirim & Soru-Cevap", Order = 32, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria5.Id, Question = "Geri bildirimler spesifik mi (\"aferin\" gibi genel mi, yoksa \"şunu iyi yaptın çünkü…\" gibi açıklayıcı mı)?", Category = "💬 Geri Bildirim & Soru-Cevap", Order = 33, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria5.Id, Question = "Öğrencilerin anladığını nasıl kontrol ediyor — anlama soruları soruyor mu?", Category = "💬 Geri Bildirim & Soru-Cevap", Order = 34, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Sınıf Yönetimi (6 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria4.Id, Question = "Sınıfta genel düzen ve sessizlik sağlanabiliyor mu?", Category = "🏫 Sınıf Yönetimi", Order = 35, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria4.Id, Question = "Derse odaklanamayan ya da konuşan öğrencilerle nasıl başa çıkıyor?", Category = "🏫 Sınıf Yönetimi", Order = 36, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria4.Id, Question = "Disiplin müdahalesi varsa nasıl yapılıyor — saygılı mı, agresif mi?", Category = "🏫 Sınıf Yönetimi", Order = 37, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria4.Id, Question = "Zamanı iyi yönetiyor mu — ders planladığı gibi ilerliyor mu?", Category = "🏫 Sınıf Yönetimi", Order = 38, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria4.Id, Question = "Geçişler (yeni konuya geçiş, aktivite değişimi) pürüzsüz mü?", Category = "🏫 Sınıf Yönetimi", Order = 39, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria4.Id, Question = "Beklenmedik durumlara (teknik sorun, öğrenci sorusu, vs.) nasıl tepki veriyor?", Category = "🏫 Sınıf Yönetimi", Order = 40, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Materyal & Araç Kullanımı (5 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria6.Id, Question = "Kullanılan materyaller (slayt, tahta, video, vs.) içerikle uyumlu ve kaliteli mi?", Category = "🖥️ Materyal & Araç Kullanımı", Order = 41, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria6.Id, Question = "Tahtayı/ekranı düzenli ve okunabilir şekilde mi kullanıyor?", Category = "🖥️ Materyal & Araç Kullanımı", Order = 42, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria6.Id, Question = "Görsel materyaller öğrenmeyi destekliyor mu, yoksa dikkat dağıtıyor mu?", Category = "🖥️ Materyal & Araç Kullanımı", Order = 43, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria6.Id, Question = "Teknolojiyi etkin mi kullanıyor?", Category = "🖥️ Materyal & Araç Kullanımı", Order = 44, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria6.Id, Question = "Materyaller tüm öğrencilerin görebileceği şekilde konumlandırılmış mı?", Category = "🖥️ Materyal & Araç Kullanımı", Order = 45, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Öğrenci Davranışı & Sınıf Atmosferi (8 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Öğrencilerin genel ilgi ve odaklanma düzeyi nasıl?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 46, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Öğrenciler ders sırasında ne yapıyor — not mu alıyor, telefonla mı ilgileniyor, konuşuyor mu?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 47, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Öğrencilerin soru sorma ya da katılım istekliliği var mı?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 48, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Sınıfta pozitif, güvenli bir öğrenme ortamı var mı?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 49, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Öğretmen-öğrenci ilişkisi nasıl görünüyor — saygılı, mesafeli, sıcak?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 50, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Öğrencilerin yüz ifadeleri ve beden dili neler söylüyor — sıkılmış mı, meraklı mı?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 51, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria8.Id, Question = "Dersin hangi anlarında öğrenci enerjisi yükseliyor ya da düşüyor?", Category = "👥 Öğrenci Davranışı & Sınıf Atmosferi", Order = 52, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Kapsayıcılık & Farklılaştırma (3 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria7.Id, Question = "Farklı öğrenme hızlarındaki öğrencilere uyum sağlıyor mu?", Category = "🌍 Kapsayıcılık & Farklılaştırma", Order = 53, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria7.Id, Question = "Öğrenme güçlüğü çeken öğrenciye özel ilgi gösteriyor mu?", Category = "🌍 Kapsayıcılık & Farklılaştırma", Order = 54, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria7.Id, Question = "Tüm öğrencilere eşit mesafede mi davranıyor, favoriler var mı?", Category = "🌍 Kapsayıcılık & Farklılaştırma", Order = 55, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },

            // Genel Ders Değerlendirmesi (6 questions)
            new() { Id = Guid.NewGuid(), CriteriaId = criteria10.Id, Question = "Bu dersi izleyen biri ne öğrendiğini açıklayabilir mi?", Category = "🔄 Genel Ders Değerlendirmesi", Order = 56, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria10.Id, Question = "Ders tekrar izlenmeli miydi, yoksa tek seferde anlaşılır mıydı?", Category = "🔄 Genel Ders Değerlendirmesi", Order = 57, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria10.Id, Question = "Öğretmenin en güçlü 3 yönü nedir?", Category = "🔄 Genel Ders Değerlendirmesi", Order = 58, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria10.Id, Question = "Öğretmenin gelişim alanları nelerdir — somut önerilerle?", Category = "🔄 Genel Ders Değerlendirmesi", Order = 59, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria10.Id, Question = "Bu ders için genel bir başarı puanı verilseydi ne olurdu (1-10)?", Category = "🔄 Genel Ders Değerlendirmesi", Order = 60, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), CriteriaId = criteria10.Id, Question = "Bir sonraki derste öncelikli olarak iyileştirilmesi gereken tek şey ne?", Category = "🔄 Genel Ders Değerlendirmesi", Order = 61, IsActive = true, CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        };
        modelBuilder.Entity<EvaluationQuestion>().HasData(questions);
    }
}
