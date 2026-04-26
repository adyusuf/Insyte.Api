using System;
using System.Collections.Generic;
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

    // ─── KULLANICILAR ──────────────────────────────────────────────────────────

    // Admin
    var adminId = await UpsertUser(context, "admin@insyte.com", "Admin@123", "Sistem", "Admin", UserRole.Admin);
    Console.WriteLine($"✓ Admin: admin@insyte.com / Admin@123");

    // Danışmanlar
    var advisor1Id = await UpsertUser(context, "mehmet.yilmaz@insyte.com", "Danisman@123", "Mehmet", "Yılmaz", UserRole.Advisor);
    var advisor2Id = await UpsertUser(context, "selin.arslan@insyte.com", "Danisman@123", "Selin", "Arslan", UserRole.Advisor);
    var advisor3Id = await UpsertUser(context, "kerem.ozturk@insyte.com", "Danisman@123", "Kerem", "Öztürk", UserRole.Advisor);
    Console.WriteLine($"✓ 3 Danışman oluşturuldu");

    // Okul Yöneticileri
    var schoolAdmin1Id = await UpsertUser(context, "yonetici@anadolu.k12.tr", "Yonetici@123", "Ayşe", "Kaya", UserRole.SchoolAdmin);
    var schoolAdmin2Id = await UpsertUser(context, "yonetici@bosporus.k12.tr", "Yonetici@123", "Hasan", "Çelik", UserRole.SchoolAdmin);
    var schoolAdmin3Id = await UpsertUser(context, "yonetici@izmir.k12.tr", "Yonetici@123", "Zeynep", "Doğan", UserRole.SchoolAdmin);
    Console.WriteLine($"✓ 3 Okul Yöneticisi oluşturuldu");

    // Öğretmenler - Okul 1 (Ankara)
    var teacher1Id  = await UpsertUser(context, "ali.demir@anadolu.k12.tr",    "Ogretmen@123", "Ali",     "Demir",    UserRole.Teacher);
    var teacher2Id  = await UpsertUser(context, "fatma.sahin@anadolu.k12.tr",  "Ogretmen@123", "Fatma",   "Şahin",    UserRole.Teacher);
    var teacher3Id  = await UpsertUser(context, "murat.koc@anadolu.k12.tr",    "Ogretmen@123", "Murat",   "Koç",      UserRole.Teacher);
    var teacher4Id  = await UpsertUser(context, "elif.aksoy@anadolu.k12.tr",   "Ogretmen@123", "Elif",    "Aksoy",    UserRole.Teacher);
    var teacher5Id  = await UpsertUser(context, "burak.polat@anadolu.k12.tr",  "Ogretmen@123", "Burak",   "Polat",    UserRole.Teacher);

    // Öğretmenler - Okul 2 (İstanbul)
    var teacher6Id  = await UpsertUser(context, "deniz.can@bosporus.k12.tr",   "Ogretmen@123", "Deniz",   "Can",      UserRole.Teacher);
    var teacher7Id  = await UpsertUser(context, "aylin.yurt@bosporus.k12.tr",  "Ogretmen@123", "Aylin",   "Yurt",     UserRole.Teacher);
    var teacher8Id  = await UpsertUser(context, "serkan.bal@bosporus.k12.tr",  "Ogretmen@123", "Serkan",  "Bal",      UserRole.Teacher);
    var teacher9Id  = await UpsertUser(context, "pinar.sen@bosporus.k12.tr",   "Ogretmen@123", "Pınar",   "Şen",      UserRole.Teacher);

    // Öğretmenler - Okul 3 (İzmir)
    var teacher10Id = await UpsertUser(context, "emre.tas@izmir.k12.tr",       "Ogretmen@123", "Emre",    "Taş",      UserRole.Teacher);
    var teacher11Id = await UpsertUser(context, "neslihan.ay@izmir.k12.tr",    "Ogretmen@123", "Neslihan","Ay",       UserRole.Teacher);
    var teacher12Id = await UpsertUser(context, "caner.kurt@izmir.k12.tr",     "Ogretmen@123", "Caner",   "Kurt",     UserRole.Teacher);
    Console.WriteLine($"✓ 12 Öğretmen oluşturuldu");

    // ─── OKULLAR ──────────────────────────────────────────────────────────────

    var school1Id = await UpsertSchool(context,
        "Anadolu Fen Lisesi",
        "Atatürk Bulvarı No:120",
        "Ankara",
        "+90 312 555 01 23",
        "iletisim@anadolu.k12.tr",
        SchoolType.Lise,
        InstitutionType.Devlet);

    var school2Id = await UpsertSchool(context,
        "Boğaziçi Özel Anadolu Lisesi",
        "Bağcılar Cad. No:55",
        "İstanbul",
        "+90 212 444 22 33",
        "info@bosporus.k12.tr",
        SchoolType.Lise,
        InstitutionType.Ozel);

    var school3Id = await UpsertSchool(context,
        "İzmir Ege İlköğretim Okulu",
        "Konak Meydanı No:8",
        "İzmir",
        "+90 232 333 44 55",
        "iletisim@izmir.k12.tr",
        SchoolType.Ilkokul,
        InstitutionType.Devlet);
    Console.WriteLine($"✓ 3 Okul oluşturuldu");

    // ─── OKUL-DANIŞMAN ATAMALARI ───────────────────────────────────────────────

    await UpsertSchoolAdvisor(context, school1Id, advisor1Id);
    await UpsertSchoolAdvisor(context, school1Id, advisor2Id);
    await UpsertSchoolAdvisor(context, school2Id, advisor2Id);
    await UpsertSchoolAdvisor(context, school2Id, advisor3Id);
    await UpsertSchoolAdvisor(context, school3Id, advisor1Id);
    await UpsertSchoolAdvisor(context, school3Id, advisor3Id);
    Console.WriteLine($"✓ Danışman atamaları yapıldı");

    // ─── OKUL-KULLANICI ATAMALARI ─────────────────────────────────────────────

    // Okul 1
    await UpsertSchoolUser(context, school1Id, schoolAdmin1Id, UserRole.SchoolAdmin);
    await UpsertSchoolUser(context, school1Id, teacher1Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school1Id, teacher2Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school1Id, teacher3Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school1Id, teacher4Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school1Id, teacher5Id, UserRole.Teacher);

    // Okul 2
    await UpsertSchoolUser(context, school2Id, schoolAdmin2Id, UserRole.SchoolAdmin);
    await UpsertSchoolUser(context, school2Id, teacher6Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school2Id, teacher7Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school2Id, teacher8Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school2Id, teacher9Id, UserRole.Teacher);

    // Okul 3
    await UpsertSchoolUser(context, school3Id, schoolAdmin3Id, UserRole.SchoolAdmin);
    await UpsertSchoolUser(context, school3Id, teacher10Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school3Id, teacher11Id, UserRole.Teacher);
    await UpsertSchoolUser(context, school3Id, teacher12Id, UserRole.Teacher);
    Console.WriteLine($"✓ Okul-kullanıcı atamaları yapıldı");

    // ─── E-POSTA DAĞITIM LİSTELERİ ───────────────────────────────────────────

    await UpsertEmailConfig(context, school1Id, "mudur@anadolu.k12.tr",       "Ahmet Müdür",      RecipientType.Principal);
    await UpsertEmailConfig(context, school1Id, "mehmet.yilmaz@insyte.com",   "Mehmet Yılmaz",    RecipientType.Advisor);
    await UpsertEmailConfig(context, school1Id, "ogrenci.isleri@anadolu.k12.tr", "Öğrenci İşleri", RecipientType.Other);

    await UpsertEmailConfig(context, school2Id, "mudur@bosporus.k12.tr",      "Selma Müdür",      RecipientType.Principal);
    await UpsertEmailConfig(context, school2Id, "selin.arslan@insyte.com",    "Selin Arslan",     RecipientType.Advisor);

    await UpsertEmailConfig(context, school3Id, "mudur@izmir.k12.tr",         "Osman Müdür",      RecipientType.Principal);
    await UpsertEmailConfig(context, school3Id, "kerem.ozturk@insyte.com",    "Kerem Öztürk",     RecipientType.Advisor);
    Console.WriteLine($"✓ E-posta dağıtım listeleri oluşturuldu");

    // ─── AI SAĞLAYICILAR ──────────────────────────────────────────────────────

    var openAiId = await UpsertAIProvider(context,
        "OpenAI",
        "openai",
        "https://api.openai.com/v1",
        "sk-proj-DEMO_ANAHTAR_BURAYA");

    var anthropicId = await UpsertAIProvider(context,
        "Anthropic",
        "anthropic",
        "https://api.anthropic.com",
        "sk-ant-DEMO_ANAHTAR_BURAYA");

    var googleId = await UpsertAIProvider(context,
        "Google AI",
        "google",
        "https://generativelanguage.googleapis.com/v1beta",
        "AIza-DEMO_ANAHTAR_BURAYA");
    Console.WriteLine($"✓ 3 AI Sağlayıcı oluşturuldu");

    // ─── AI MODELLER ──────────────────────────────────────────────────────────

    var gpt4oId      = await UpsertAIModel(context, openAiId,    "GPT-4o",           "gpt-4o",                    128000);
    var gpt4oMiniId  = await UpsertAIModel(context, openAiId,    "GPT-4o Mini",      "gpt-4o-mini",               128000);
    var claudeSonnet = await UpsertAIModel(context, anthropicId, "Claude Sonnet 4",  "claude-sonnet-4-5-20251015", 200000);
    var claudeHaiku  = await UpsertAIModel(context, anthropicId, "Claude Haiku 4",   "claude-haiku-4-5",          200000);
    var geminiPro    = await UpsertAIModel(context, googleId,    "Gemini 2.5 Pro",   "gemini-2.5-pro",            2000000);
    Console.WriteLine($"✓ 5 AI Model oluşturuldu");

    // ─── DEĞERLENDİRME KRİTERLERİ ────────────────────────────────────────────

    var criteria1Id = await UpsertCriteria(context,
        "Genel Ders Değerlendirme",
        "Herhangi bir ders için uygulanabilecek genel değerlendirme kriterleri seti. Ders anlatımı, sınıf yönetimi, öğrenci etkileşimi ve pedagojik yaklaşımları kapsar.",
        """
        Sen deneyimli bir eğitim danışmanısın. Sana gönderilen ders videosunu aşağıdaki kriterlere göre değerlendir.

        Değerlendirme alanları:
        1. DERS PLANLAMA VE HAZIRLIK
           - Dersin hedefleri açık mı?
           - İçerik seviyeye uygun mu?
           - Materyal kullanımı yeterli mi?

        2. ÖĞRETME STRATEJİLERİ
           - Açıklama yöntemleri etkili mi?
           - Örnek çeşitliliği var mı?
           - Farklı öğrenme stillerine hitap ediyor mu?

        3. SINIF YÖNETİMİ
           - Zaman yönetimi nasıl?
           - Disiplin sağlanmış mı?
           - Öğrenci katılımı nasıl yönetiliyor?

        4. ÖĞRENCİ ETKİLEŞİMİ
           - Soru-cevap etkinliği var mı?
           - Öğrenciler aktif mi?
           - Geri bildirim veriliyor mu?

        5. İLETİŞİM BECERİLERİ
           - Dil açık ve anlaşılır mı?
           - Ses tonu ve beden dili uygun mu?
           - Öğrencilerle iletişim kalitesi nasıl?

        Her alanı 1-10 arasında puanla. Güçlü yönler ve geliştirme önerileri yaz.
        Türkçe, profesyonel ve yapıcı bir dil kullan.

        Yanıtını şu JSON formatında ver:
        {
          "genelPuan": <1-10 arası sayı>,
          "dersPlanlama": { "puan": <1-10>, "yorumlar": "<metin>", "gucluYonler": ["<madde>"], "gelistirmeOnerisi": ["<madde>"] },
          "ogretmeStrategisi": { "puan": <1-10>, "yorumlar": "<metin>", "gucluYonler": ["<madde>"], "gelistirmeOnerisi": ["<madde>"] },
          "sinifYonetimi": { "puan": <1-10>, "yorumlar": "<metin>", "gucluYonler": ["<madde>"], "gelistirmeOnerisi": ["<madde>"] },
          "ogrenciEtkiselimi": { "puan": <1-10>, "yorumlar": "<metin>", "gucluYonler": ["<madde>"], "gelistirmeOnerisi": ["<madde>"] },
          "iletisimBecerileri": { "puan": <1-10>, "yorumlar": "<metin>", "gucluYonler": ["<madde>"], "gelistirmeOnerisi": ["<madde>"] },
          "genelYorum": "<genel değerlendirme metni>",
          "oncelikliGelistirmeler": ["<madde1>", "<madde2>", "<madde3>"]
        }
        """,
        null);

    var criteria2Id = await UpsertCriteria(context,
        "Matematik Dersi Özel Değerlendirme",
        "Matematik derslerine özgü değerlendirme kriterleri. Problem çözme süreçleri, matematiksel düşünme ve kavramsal anlama odaklıdır.",
        """
        Sen matematik eğitimi uzmanı bir danışmansın. Matematik dersini derinlemesine değerlendirmek için aşağıdaki kriterleri kullan.

        1. MATEMATİKSEL İÇERİK DOĞRULUĞU
           - Kavramlar doğru mu anlatılıyor?
           - Formüller ve teoremler hatasız mı?
           - Örnekler matematiksel açıdan tutarlı mı?

        2. PROBLEM ÇÖZME SÜRECİ
           - Adım adım çözüm mantıklı mı?
           - Alternatif çözüm yolları gösteriliyor mu?
           - Hata yönetimi nasıl?

        3. SOYUTTAN SOMUTA GEÇİŞ
           - Soyut kavramlar somutlaştırılıyor mu?
           - Gerçek hayat bağlantısı kurulmuş mu?
           - Görselleştirme kullanılmış mı?

        4. ÖĞRENCİ ANLAMASINI KONTROL
           - Sorular anlama odaklı mı?
           - Yanlış anlamaları tespit ediyor mu?
           - Farklı seviyelere hitap ediyor mu?

        5. MÜFREDAT UYUMU
           - Kazanımlar karşılanmış mı?
           - Seviyeye uygun zorluk?
           - Sınav odaklı mı, anlama odaklı mı?

        JSON formatı:
        {
          "genelPuan": <1-10>,
          "matematikselIcerik": { "puan": <1-10>, "yorumlar": "<metin>", "hatalar": ["<hata>"], "olumlu": ["<madde>"] },
          "problemCozme": { "puan": <1-10>, "yorumlar": "<metin>", "yontemler": ["<yontem>"], "gelistirme": ["<madde>"] },
          "soyutSomut": { "puan": <1-10>, "yorumlar": "<metin>", "ornekler": ["<ornek>"] },
          "anlayisKontrolu": { "puan": <1-10>, "yorumlar": "<metin>", "teknikler": ["<teknik>"] },
          "mufredatUyumu": { "puan": <1-10>, "yorumlar": "<metin>", "kazanimlar": ["<kazanim>"] },
          "genelYorum": "<metin>",
          "kritikGelistirmeler": ["<madde1>", "<madde2>"]
        }
        """,
        "Matematik");

    var criteria3Id = await UpsertCriteria(context,
        "Sınıf Yönetimi Odaklı Değerlendirme",
        "Sınıf dinamiklerini, disiplini, katılım yönetimini ve öğrenci davranışlarını inceleyen özel kriter seti.",
        """
        Sen sınıf yönetimi konusunda uzmanlaşmış bir eğitim danışmanısın. Bu videoyu sınıf yönetimi odaklı değerlendir.

        1. FİZİKSEL ORTAM KULLANIMI
           - Sınıf düzeni uygun mu?
           - Öğretmen hareketi ve konumlanması?
           - Materyaller erişilebilir mi?

        2. ZAMAN YÖNETİMİ
           - Ders akışı düzenli mi?
           - Geçişler hızlı mı?
           - Bekleme süreleri minimize edilmiş mi?

        3. DAVRANIŞSAL YÖNETİM
           - İstenmeyen davranışlara tepkiler?
           - Olumlu pekiştireç kullanımı?
           - Ödül/sonuç sistemi var mı?

        4. KATILIM STRATEJİLERİ
           - Tüm öğrenciler kapsanmış mı?
           - Pasif öğrenciler aktive ediliyor mu?
           - Grup dinamikleri iyi mi?

        5. KURAL VE RUTIN OLUŞTURMA
           - Kurallar açık mı?
           - Rutinler oluşturulmuş mu?
           - Öğrenciler süreci biliyor mu?

        JSON formatı:
        {
          "genelPuan": <1-10>,
          "fizikselOrtam": { "puan": <1-10>, "yorumlar": "<metin>", "iyilestirmeler": ["<madde>"] },
          "zamanYonetimi": { "puan": <1-10>, "yorumlar": "<metin>", "guclukler": ["<madde>"], "oneriler": ["<madde>"] },
          "davranisYonetimi": { "puan": <1-10>, "yorumlar": "<metin>", "teknikler": ["<teknik>"] },
          "katilimStrategisi": { "puan": <1-10>, "yorumlar": "<metin>", "ornekler": ["<ornek>"] },
          "kuralRutin": { "puan": <1-10>, "yorumlar": "<metin>", "eksikler": ["<eksik>"] },
          "genelYorum": "<metin>",
          "acilMudahale": ["<madde>"],
          "uzunVadeliPlan": ["<madde>"]
        }
        """,
        null);
    Console.WriteLine($"✓ 3 Değerlendirme Kriteri oluşturuldu");

    // ─── KRİTER SORULARI ──────────────────────────────────────────────────────

    await UpsertQuestion(context, criteria1Id, "Öğretmen dersin hedeflerini açıkça belirtiyor mu?",              "Ders Planlama",    1);
    await UpsertQuestion(context, criteria1Id, "İçerik öğrencilerin seviyesine uygun mu?",                       "Ders Planlama",    2);
    await UpsertQuestion(context, criteria1Id, "Ders materyalleri etkili biçimde kullanılmış mı?",               "Ders Planlama",    3);
    await UpsertQuestion(context, criteria1Id, "Açıklama yöntemleri anlaşılır ve çeşitli mi?",                   "Öğretme",          4);
    await UpsertQuestion(context, criteria1Id, "Öğrencilerle yeterli etkileşim var mı?",                         "Etkileşim",        5);
    await UpsertQuestion(context, criteria1Id, "Sınıfta zaman etkin biçimde kullanılıyor mu?",                   "Sınıf Yönetimi",   6);
    await UpsertQuestion(context, criteria1Id, "Öğrencilerin soruları ve katılımı nasıl yönetiliyor?",           "Etkileşim",        7);
    await UpsertQuestion(context, criteria1Id, "Geri bildirim kalitesi ve zamanlaması uygun mu?",                "İletişim",         8);

    await UpsertQuestion(context, criteria2Id, "Matematiksel kavramlar doğru ve eksiksiz anlatılmış mı?",        "İçerik",           1);
    await UpsertQuestion(context, criteria2Id, "Problem çözme adımları açık ve takip edilebilir mi?",            "Problem Çözme",    2);
    await UpsertQuestion(context, criteria2Id, "Soyut kavramlar somut örneklerle destekleniyor mu?",             "Pedagoji",         3);
    await UpsertQuestion(context, criteria2Id, "Öğrenci anlamasını ölçen sorular soruluyor mu?",                 "Değerlendirme",    4);
    await UpsertQuestion(context, criteria2Id, "Müfredat kazanımlarına uyum sağlanmış mı?",                      "Müfredat",         5);

    await UpsertQuestion(context, criteria3Id, "Sınıf fiziksel düzeni öğrenmeye uygun mu?",                      "Fiziksel Ortam",   1);
    await UpsertQuestion(context, criteria3Id, "Ders geçişleri hızlı ve akıcı mı?",                             "Zaman",            2);
    await UpsertQuestion(context, criteria3Id, "İstenmeyen davranışlara yapıcı tepkiler veriliyor mu?",          "Davranış",         3);
    await UpsertQuestion(context, criteria3Id, "Tüm öğrenciler derse katılıma teşvik ediliyor mu?",             "Katılım",          4);
    await UpsertQuestion(context, criteria3Id, "Sınıf kuralları ve rutinler öğrenciler tarafından biliniyor mu?","Kural",            5);
    Console.WriteLine($"✓ Kriter soruları oluşturuldu");

    // ─── SINIFLAR ─────────────────────────────────────────────────────────────

    var class9A   = await UpsertClass(context, school1Id, "9-A",   ClassLevel.Level9,  "A");
    var class9B   = await UpsertClass(context, school1Id, "9-B",   ClassLevel.Level9,  "B");
    var class10A  = await UpsertClass(context, school1Id, "10-A",  ClassLevel.Level10, "A");
    var class11A  = await UpsertClass(context, school1Id, "11-A",  ClassLevel.Level11, "A");
    var class12A  = await UpsertClass(context, school1Id, "12-A",  ClassLevel.Level12, "A");

    var classB9A  = await UpsertClass(context, school2Id, "9-A",   ClassLevel.Level9,  "A");
    var classB10A = await UpsertClass(context, school2Id, "10-A",  ClassLevel.Level10, "A");
    var classB11A = await UpsertClass(context, school2Id, "11-A",  ClassLevel.Level11, "A");

    var classC3A  = await UpsertClass(context, school3Id, "3-A",   ClassLevel.Level3,  "A");
    var classC4A  = await UpsertClass(context, school3Id, "4-A",   ClassLevel.Level4,  "A");
    var classC5A  = await UpsertClass(context, school3Id, "5-A",   ClassLevel.Level5,  "A");
    Console.WriteLine($"✓ 11 Sınıf oluşturuldu");

    // ─── DERSLER ──────────────────────────────────────────────────────────────

    var subjectMath    = await UpsertSubject(context, school1Id, "Matematik",           "Matematik",       "9-12", 5);
    var subjectPhysics = await UpsertSubject(context, school1Id, "Fizik",               "Fen Bilimleri",   "9-12", 3);
    var subjectTurkish = await UpsertSubject(context, school1Id, "Türk Dili ve Edebiyatı", "Dil",         "9-12", 4);
    var subjectHistory = await UpsertSubject(context, school1Id, "Tarih",               "Sosyal Bilimler", "9-12", 2);
    var subjectEnglish = await UpsertSubject(context, school1Id, "İngilizce",           "Yabancı Dil",     "9-12", 4);

    var subjectB_Math  = await UpsertSubject(context, school2Id, "Matematik",           "Matematik",       "9-11", 5);
    var subjectB_Chem  = await UpsertSubject(context, school2Id, "Kimya",               "Fen Bilimleri",   "9-11", 3);
    var subjectB_Eng   = await UpsertSubject(context, school2Id, "İngilizce",           "Yabancı Dil",     "9-11", 6);

    var subjectC_Math  = await UpsertSubject(context, school3Id, "Matematik",           "Matematik",       "3-5",  4);
    var subjectC_Turk  = await UpsertSubject(context, school3Id, "Türkçe",              "Dil",             "3-5",  5);
    var subjectC_Sci   = await UpsertSubject(context, school3Id, "Fen ve Teknoloji",    "Fen",             "3-5",  3);
    Console.WriteLine($"✓ 11 Ders oluşturuldu");

    // ─── VİDEOLAR ─────────────────────────────────────────────────────────────

    // Video 1 - Tamamlanmış değerlendirme
    var video1Id = await UpsertVideo(context,
        "9-A Matematik - Türev Giriş",
        "videos/school1/video_001.mp4",
        "video_001.mp4",
        524_288_000L, // ~500MB
        school1Id, teacher1Id, adminId,
        "Matematik",
        VideoStatus.Evaluated);

    // Video 2 - Onaylanmış
    var video2Id = await UpsertVideo(context,
        "10-A Fizik - Newton'un Hareket Yasaları",
        "videos/school1/video_002.mp4",
        "video_002.mp4",
        734_003_200L, // ~700MB
        school1Id, teacher2Id, schoolAdmin1Id,
        "Fizik",
        VideoStatus.Approved);

    // Video 3 - Yüklendi, henüz değerlendirilmedi
    var video3Id = await UpsertVideo(context,
        "9-B Türk Edebiyatı - Tanzimat Dönemi",
        "videos/school1/video_003.mp4",
        "video_003.mp4",
        412_316_672L, // ~393MB
        school1Id, teacher3Id, teacher3Id,
        "Türk Dili ve Edebiyatı",
        VideoStatus.Uploaded);

    // Video 4 - İstanbul okulu
    var video4Id = await UpsertVideo(context,
        "9-A İngilizce - Present Perfect Tense",
        "videos/school2/video_004.mp4",
        "video_004.mp4",
        629_145_600L, // ~600MB
        school2Id, teacher6Id, schoolAdmin2Id,
        "İngilizce",
        VideoStatus.Approved);

    // Video 5 - İstanbul okulu, değerlendirme bekliyor
    var video5Id = await UpsertVideo(context,
        "10-A Kimya - Mol Kavramı",
        "videos/school2/video_005.mp4",
        "video_005.mp4",
        943_718_400L, // ~900MB
        school2Id, teacher7Id, teacher7Id,
        "Kimya",
        VideoStatus.Evaluated);

    // Video 6 - İzmir okulu
    var video6Id = await UpsertVideo(context,
        "4-A Matematik - Bölme İşlemi",
        "videos/school3/video_006.mp4",
        "video_006.mp4",
        314_572_800L, // ~300MB
        school3Id, teacher10Id, schoolAdmin3Id,
        "Matematik",
        VideoStatus.Approved);

    // Video 7 - Başarısız işlem
    var video7Id = await UpsertVideo(context,
        "5-A Fen ve Teknoloji - Kuvvet ve Hareket",
        "videos/school3/video_007.mp4",
        "video_007.mp4",
        471_859_200L, // ~450MB
        school3Id, teacher11Id, teacher11Id,
        "Fen ve Teknoloji",
        VideoStatus.Uploaded);
    Console.WriteLine($"✓ 7 Video oluşturuldu");

    // ─── DEĞERLENDİRMELER ─────────────────────────────────────────────────────

    var eval1Id = await UpsertEvaluation(context,
        video1Id, criteria2Id, gpt4oId,
        EvaluationStatus.Completed,
        """
        {
          "genelPuan": 7.8,
          "matematikselIcerik": {
            "puan": 8.5,
            "yorumlar": "Türev kavramı matematiksel açıdan doğru ve eksiksiz anlatılmış. Limit tanımından türeve geçiş mantıksal sırayla yapılmış.",
            "hatalar": [],
            "olumlu": ["Formal tanım önce verilmiş", "Notasyon tutarlı kullanılmış", "Örnekler doğru seçilmiş"]
          },
          "problemCozme": {
            "puan": 7.5,
            "yorumlar": "Problem çözme adımları açık ancak bazı ara adımlar hızlı geçilmiş. Öğrencilerin takip etmesi zorlanabilir.",
            "yontemler": ["Limit tanımıyla doğrudan hesap", "Kural uygulaması"],
            "gelistirme": ["Ara adımları daha yavaş geçin", "Orta seviye öğrenci için ek açıklamalar ekleyin"]
          },
          "soyutSomut": {
            "puan": 7.0,
            "yorumlar": "Türev kavramı grafik üzerinde iyi gösterilmiş. Gerçek hayat bağlantısı (hız-konum) kısaca değinilmiş ama geliştirilebilir.",
            "ornekler": ["Teğet eğimi grafiği", "Anlık hız örneği"]
          },
          "anlayisKontrolu": {
            "puan": 8.0,
            "yorumlar": "Düzenli soru sorma alışkanlığı var. Öğrencilerin yanlış anlamalarını yakalamak için iyi sorular sorulmuş.",
            "teknikler": ["Tahmin soruları", "Hata bulma egzersizi", "Sözlü kontrol"]
          },
          "mufredatUyumu": {
            "puan": 8.0,
            "yorumlar": "9. sınıf müfredatıyla tam uyumlu. Kazanımlar büyük ölçüde karşılanmış.",
            "kazanimlar": ["Türev tanımı", "Türev hesaplama kuralları", "Polinom türevi"]
          },
          "genelYorum": "Ali Demir öğretmen, türev konusunu sağlam matematiksel temelle anlatmıştır. Problem çözme adımlarını biraz daha yavaş ve ayrıntılı anlatması, gerçek hayat bağlantılarını güçlendirmesi önerilir. Genel olarak başarılı bir ders.",
          "kritikGelistirmeler": ["Ara adımları daha yavaş geçin", "Fizik/mühendislik bağlantısını güçlendirin"]
        }
        """,
        3245, 1812);

    var eval2Id = await UpsertEvaluation(context,
        video2Id, criteria1Id, claudeSonnet,
        EvaluationStatus.Completed,
        """
        {
          "genelPuan": 8.9,
          "dersPlanlama": {
            "puan": 9.0,
            "yorumlar": "Ders hedefleri açıkça belirtilmiş, içerik seviyeye çok uygun. Materyal kullanımı mükemmel.",
            "gucluYonler": ["Net hedef belirleme", "Uygun içerik seçimi", "Etkili materyal kullanımı"],
            "gelistirmeOnerisi": ["Ders sonu özet slaydı eklenebilir"]
          },
          "ogretmeStrategisi": {
            "puan": 8.5,
            "yorumlar": "Newton yasaları somut deneylerle pekiştirilmiş. Farklı öğrenme stillerine hitap ediliyor.",
            "gucluYonler": ["Deney destekli anlatım", "Günlük hayat örnekleri", "Adım adım açıklama"],
            "gelistirmeOnerisi": ["Daha fazla öğrenci deneyi eklenebilir"]
          },
          "sinifYonetimi": {
            "puan": 9.0,
            "yorumlar": "Sınıf kontrolü çok iyi. Zaman yönetimi mükemmel, hiç boşluk yok.",
            "gucluYonler": ["Akıcı ders akışı", "Etkin zaman kullanımı", "Disiplinli ortam"],
            "gelistirmeOnerisi": []
          },
          "ogrenciEtkiselimi": {
            "puan": 9.0,
            "yorumlar": "Öğrenciler aktif olarak katılmış. Soru-cevap yoğun ve verimli.",
            "gucluYonler": ["Yüksek katılım oranı", "Kaliteli soru-cevap", "Öğrenci fikirleri değerlendiriliyor"],
            "gelistirmeOnerisi": ["Küçük grup çalışmaları eklenebilir"]
          },
          "iletisimBecerileri": {
            "puan": 9.0,
            "yorumlar": "Açık ve anlaşılır dil. Ses tonu uygun, beden dili etkili.",
            "gucluYonler": ["Açık anlatım", "Güçlü beden dili", "Öğrencilerle göz teması"],
            "gelistirmeOnerisi": []
          },
          "genelYorum": "Fatma Şahin öğretmen üst düzey bir ders sunumu gerçekleştirmiştir. Newton yasalarını deney destekli, öğrenci katılımı yüksek bir ortamda işlemiştir. Bu ders meslektaşlar için model niteliğindedir.",
          "oncelikliGelistirmeler": ["Öğrenci deneyleri artırılabilir", "Ders sonu yazılı özet eklenebilir", "Çapraz bağlantılar güçlendirilebilir"]
        }
        """,
        4102, 2391);

    var eval3Id = await UpsertEvaluation(context,
        video4Id, criteria1Id, claudeSonnet,
        EvaluationStatus.Completed,
        """
        {
          "genelPuan": 8.2,
          "dersPlanlama": {
            "puan": 8.0,
            "yorumlar": "Ders yapısı iyi planlanmış. Present Perfect'in kullanım alanları sistematik biçimde ele alınmış.",
            "gucluYonler": ["Sistematik konu akışı", "Yeterli alıştırma zamanı", "Görsel destekli sunum"],
            "gelistirmeOnerisi": ["Bağlamsal örnekler daha güçlü olabilir"]
          },
          "ogretmeStrategisi": {
            "puan": 8.5,
            "yorumlar": "Kalıpları bağlamsal cümlelerle öğretme yaklaşımı etkili. Alıştırma çeşitliliği iyi.",
            "gucluYonler": ["Bağlam odaklı öğretim", "Çeşitli alıştırmalar", "Hatayı düzeltme teknikleri"],
            "gelistirmeOnerisi": ["Daha fazla özgün üretim (speaking) zamanı"]
          },
          "sinifYonetimi": {
            "puan": 8.0,
            "yorumlar": "Sınıf dili yönetimi iyi. İngilizce sınıf dilini tutma oranı yüksek.",
            "gucluYonler": ["İngilizce sınıf dili", "Akıcı geçişler", "Katılım yönetimi"],
            "gelistirmeOnerisi": ["Bazı öğrenciler pasif kalıyor"]
          },
          "ogrenciEtkiselimi": {
            "puan": 8.0,
            "yorumlar": "Çift çalışma ve grup etkinlikleri var. Katılım genel olarak iyi.",
            "gucluYonler": ["Çift çalışma etkinlikleri", "Sözlü üretim fırsatları"],
            "gelistirmeOnerisi": ["Pasif öğrencileri daha fazla kapsayın"]
          },
          "iletisimBecerileri": {
            "puan": 8.5,
            "yorumlar": "Telaffuz açık ve anlaşılır. Hataları düzeltme stili öğrencileri rahatsız etmiyor.",
            "gucluYonler": ["Net telaffuz", "Yapıcı hata düzeltme", "Pozitif atmosfer"],
            "gelistirmeOnerisi": []
          },
          "genelYorum": "Deniz Can öğretmen Present Perfect konusunu akıcı ve etkili biçimde işlemiştir. Bağlamsal öğretim yaklaşımı güçlü. Pasif öğrencilerin sürece dahil edilmesi ve daha fazla özgün dil üretim fırsatı yaratılması öncelikli geliştirme alanları.",
          "oncelikliGelistirmeler": ["Pasif öğrencileri daha fazla kapsayın", "Özgün üretim (speaking) zamanını artırın", "Bağlamsal örnekleri zenginleştirin"]
        }
        """,
        3890, 2105);

    var eval4Id = await UpsertEvaluation(context,
        video5Id, criteria1Id, gpt4oMiniId,
        EvaluationStatus.Completed,
        """
        {
          "genelPuan": 7.2,
          "dersPlanlama": {
            "puan": 7.0,
            "yorumlar": "Mol kavramı için temel yapı var ama bağlantılar yeterince kurulmamış.",
            "gucluYonler": ["Temel kavramlar işlenmiş", "Formüller doğru"],
            "gelistirmeOnerisi": ["Günlük hayat bağlantısı eksik", "Görsel destek yetersiz"]
          },
          "ogretmeStrategisi": {
            "puan": 7.0,
            "yorumlar": "Açıklama mantıksal ama soyut kalıyor. Somutlaştırma az.",
            "gucluYonler": ["Mantıksal sıra", "Formül aktarımı başarılı"],
            "gelistirmeOnerisi": ["Somut modeller kullanın", "Görsel sunum güçlendirilmeli"]
          },
          "sinifYonetimi": {
            "puan": 7.5,
            "yorumlar": "Sınıf genel olarak düzenli. Bazı dikkat dağınıklıkları var.",
            "gucluYonler": ["Düzenli ortam", "Zaman yönetimi makul"],
            "gelistirmeOnerisi": ["Dikkat dağıtan unsurları minimize edin"]
          },
          "ogrenciEtkiselimi": {
            "puan": 7.0,
            "yorumlar": "Soru-cevap var ama yüzeysel. Öğrenci düşünmeye zorlanmıyor.",
            "gucluYonler": ["Düzenli soru sorma", "Gönüllü katılım var"],
            "gelistirmeOnerisi": ["Açık uçlu sorular artırın", "Tahmin/hipotez soruları ekleyin"]
          },
          "iletisimBecerileri": {
            "puan": 7.5,
            "yorumlar": "Dil açık. Hız zaman zaman yüksek. Ses tonu monoton.",
            "gucluYonler": ["Anlaşılır dil", "Türkçe kimya terimleri doğru"],
            "gelistirmeOnerisi": ["Anlatım hızını ayarlayın", "Ses tonunu çeşitlendirin"]
          },
          "genelYorum": "Aylin Yurt öğretmen mol kavramını temel düzeyde aktarmayı başarmıştır. Görsel destek, somut modeller ve daha derinlemesine öğrenci katılımı dersi önemli ölçüde güçlendirecektir.",
          "oncelikliGelistirmeler": ["Görsel ve somut model kullanımını artırın", "Öğrencileri daha derin düşünmeye zorlayın", "Anlatım hızını ve ton çeşitliliğini geliştirin"]
        }
        """,
        2876, 1654);

    var eval5Id = await UpsertEvaluation(context,
        video6Id, criteria1Id, geminiPro,
        EvaluationStatus.Completed,
        """
        {
          "genelPuan": 9.1,
          "dersPlanlama": {
            "puan": 9.5,
            "yorumlar": "4. sınıf bölme işlemi için mükemmel hazırlanmış ders. Basamaklı zorluk seviyesi çok iyi.",
            "gucluYonler": ["Basamaklı zorluk", "Çeşitli materyal", "Net kazanımlar"],
            "gelistirmeOnerisi": []
          },
          "ogretmeStrategisi": {
            "puan": 9.0,
            "yorumlar": "Somut nesnelerden soyuta geçiş başarılı. Manipülatifler çok etkili kullanılmış.",
            "gucluYonler": ["Somuttan soyuta", "Manipülatif kullanımı", "Çeşitli gösterim yöntemleri"],
            "gelistirmeOnerisi": ["Hata analizi kısmı güçlendirilebilir"]
          },
          "sinifYonetimi": {
            "puan": 9.0,
            "yorumlar": "İlkokul sınıfını büyük bir ustalıkla yönetmekte. Rutinler oturmuş.",
            "gucluYonler": ["Güçlü rutinler", "Pozitif pekiştirme", "Aktif katılım yönetimi"],
            "gelistirmeOnerisi": []
          },
          "ogrenciEtkiselimi": {
            "puan": 9.0,
            "yorumlar": "Çocuklar çok aktif ve hevesli. Oyun temelli öğrenme unsurları var.",
            "gucluYonler": ["Yüksek motivasyon", "Oyunlaştırma", "Gönüllü katılım yüksek"],
            "gelistirmeOnerisi": ["Değerlendirme kısmına daha fazla zaman"]
          },
          "iletisimBecerileri": {
            "puan": 9.0,
            "yorumlar": "Çocuklara uygun, sıcak ve teşvik edici iletişim. Övgü dengelenmiş.",
            "gucluYonler": ["Yaşa uygun dil", "Sıcak iletişim", "Dengeli övgü"],
            "gelistirmeOnerisi": []
          },
          "genelYorum": "Emre Taş öğretmen, 4. sınıf seviyesinde bölme işlemini son derece etkili ve yaratıcı biçimde öğretmiştir. Manipülatif kullanımı ve oyunlaştırma öğeleri dersi öğrenciler için hem eğlenceli hem de verimli kılmıştır. Meslektaşlarla paylaşılmaya değer bir ders örneğidir.",
          "oncelikliGelistirmeler": ["Değerlendirme/öz değerlendirme kısmına zaman ayırın", "Hata analizi aktiviteleri ekleyin"]
        }
        """,
        5123, 2987);
    Console.WriteLine($"✓ 5 Değerlendirme oluşturuldu");

    // ─── RAPORLAR ─────────────────────────────────────────────────────────────

    // Rapor 1 - Onay bekliyor (video1, eval1)
    await UpsertReport(context, eval1Id, null, null, ReportStatus.Draft);

    // Rapor 2 - Onaylanmış (video2, eval2)
    await UpsertReport(context, eval2Id, adminId,
        DateTime.UtcNow.AddDays(-5),
        ReportStatus.Approved,
        "reports/report_002.pdf");

    // Rapor 3 - Gönderilmiş (video4, eval3)
    await UpsertReport(context, eval3Id, advisor2Id,
        DateTime.UtcNow.AddDays(-12),
        ReportStatus.Sent,
        "reports/report_003.pdf");

    // Rapor 4 - Onay bekliyor (video5, eval4)
    await UpsertReport(context, eval4Id, null, null, ReportStatus.Draft);

    // Rapor 5 - Gönderilmiş (video6, eval5)
    await UpsertReport(context, eval5Id, advisor1Id,
        DateTime.UtcNow.AddDays(-3),
        ReportStatus.Sent,
        "reports/report_005.pdf");
    Console.WriteLine($"✓ 5 Rapor oluşturuldu");

    // ─── ÖZET ─────────────────────────────────────────────────────────────────

    Console.WriteLine("\n=== Seed Tamamlandı ===\n");
    Console.WriteLine("── Kullanıcılar ──────────────────────────");
    Console.WriteLine("  Admin:              admin@insyte.com / Admin@123");
    Console.WriteLine("  Danışman 1:         mehmet.yilmaz@insyte.com / Danisman@123");
    Console.WriteLine("  Danışman 2:         selin.arslan@insyte.com / Danisman@123");
    Console.WriteLine("  Danışman 3:         kerem.ozturk@insyte.com / Danisman@123");
    Console.WriteLine("  Okul Yön. 1 (ANK):  yonetici@anadolu.k12.tr / Yonetici@123");
    Console.WriteLine("  Okul Yön. 2 (İST):  yonetici@bosporus.k12.tr / Yonetici@123");
    Console.WriteLine("  Okul Yön. 3 (İZM):  yonetici@izmir.k12.tr / Yonetici@123");
    Console.WriteLine("  Öğretmenler (12x):  ogretmen1@anadolu.k12.tr ... / Ogretmen@123");
    Console.WriteLine("\n── Okullar ───────────────────────────────");
    Console.WriteLine("  1. Anadolu Fen Lisesi — Ankara (Devlet)");
    Console.WriteLine("  2. Boğaziçi Özel Anadolu Lisesi — İstanbul (Özel)");
    Console.WriteLine("  3. İzmir Ege İlköğretim Okulu — İzmir (Devlet)");
    Console.WriteLine("\n── AI Sağlayıcılar ───────────────────────");
    Console.WriteLine("  OpenAI (gpt-4o, gpt-4o-mini)");
    Console.WriteLine("  Anthropic (claude-sonnet-4-5, claude-haiku-4-5)");
    Console.WriteLine("  Google AI (gemini-2.5-pro)");
    Console.WriteLine("\n── Değerlendirme Durumları ───────────────");
    Console.WriteLine("  5 Değerlendirme (hepsi Completed)");
    Console.WriteLine("  5 Rapor: 2 Draft (onay bekliyor), 1 Approved, 2 Sent");
}

// ═══════════════════════════════════════════════════════════════════════════
// Helper fonksiyonlar
// ═══════════════════════════════════════════════════════════════════════════

static async Task<Guid> UpsertUser(InsyteDbContext ctx, string email, string password,
    string firstName, string lastName, UserRole role)
{
    var existing = await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);
    if (existing != null) return existing.Id;

    var user = new User
    {
        Id           = Guid.NewGuid(),
        Email        = email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
        FirstName    = firstName,
        LastName     = lastName,
        Role         = role,
        IsActive     = true,
        CreatedAt    = DateTime.UtcNow
    };
    ctx.Users.Add(user);
    await ctx.SaveChangesAsync();
    return user.Id;
}

static async Task<Guid> UpsertSchool(InsyteDbContext ctx, string name, string address,
    string city, string phone, string email, SchoolType schoolType, InstitutionType institutionType)
{
    var existing = await ctx.Schools.FirstOrDefaultAsync(s => s.Name == name);
    if (existing != null) return existing.Id;

    var school = new School
    {
        Id              = Guid.NewGuid(),
        Name            = name,
        Address         = address,
        City            = city,
        Phone           = phone,
        Email           = email,
        SchoolType      = schoolType,
        InstitutionType = institutionType,
        IsActive        = true,
        CreatedAt       = DateTime.UtcNow
    };
    ctx.Schools.Add(school);
    await ctx.SaveChangesAsync();
    return school.Id;
}

static async Task UpsertSchoolAdvisor(InsyteDbContext ctx, Guid schoolId, Guid userId)
{
    var exists = await ctx.SchoolAdvisors.AnyAsync(sa => sa.SchoolId == schoolId && sa.UserId == userId);
    if (exists) return;

    ctx.SchoolAdvisors.Add(new SchoolAdvisor
    {
        Id         = Guid.NewGuid(),
        SchoolId   = schoolId,
        UserId     = userId,
        AssignedAt = DateTime.UtcNow
    });
    await ctx.SaveChangesAsync();
}

static async Task UpsertSchoolUser(InsyteDbContext ctx, Guid schoolId, Guid userId, UserRole role)
{
    var exists = await ctx.SchoolUsers.AnyAsync(su => su.SchoolId == schoolId && su.UserId == userId);
    if (exists) return;

    ctx.SchoolUsers.Add(new SchoolUser
    {
        Id         = Guid.NewGuid(),
        SchoolId   = schoolId,
        UserId     = userId,
        Role       = role,
        AssignedAt = DateTime.UtcNow
    });
    await ctx.SaveChangesAsync();
}

static async Task UpsertEmailConfig(InsyteDbContext ctx, Guid schoolId,
    string email, string? name, RecipientType type)
{
    var exists = await ctx.EmailConfigs.AnyAsync(ec => ec.SchoolId == schoolId && ec.RecipientEmail == email);
    if (exists) return;

    ctx.EmailConfigs.Add(new EmailConfig
    {
        Id             = Guid.NewGuid(),
        SchoolId       = schoolId,
        RecipientEmail = email,
        RecipientName  = name,
        RecipientType  = type,
        IsActive       = true,
        CreatedAt      = DateTime.UtcNow
    });
    await ctx.SaveChangesAsync();
}

static async Task<Guid> UpsertAIProvider(InsyteDbContext ctx, string name,
    string provider, string? baseUrl, string? apiKey)
{
    var existing = await ctx.AIProviders.FirstOrDefaultAsync(p => p.Name == name);
    if (existing != null) return existing.Id;

    var entity = new AIProvider
    {
        Id        = Guid.NewGuid(),
        Name      = name,
        Provider  = provider,
        BaseUrl   = baseUrl,
        ApiKey    = apiKey, // Gerçek ortamda şifreli saklanır
        IsActive  = true,
        CreatedAt = DateTime.UtcNow
    };
    ctx.AIProviders.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task<Guid> UpsertAIModel(InsyteDbContext ctx, Guid providerId,
    string name, string modelId, int maxTokens)
{
    var existing = await ctx.AIModels.FirstOrDefaultAsync(m => m.ModelId == modelId);
    if (existing != null) return existing.Id;

    var entity = new AIModel
    {
        Id           = Guid.NewGuid(),
        AIProviderId = providerId,
        Name         = name,
        ModelId      = modelId,
        MaxTokens    = maxTokens,
        IsActive     = true,
        CreatedAt    = DateTime.UtcNow
    };
    ctx.AIModels.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task<Guid> UpsertCriteria(InsyteDbContext ctx, string name,
    string? description, string instructions, string? subject)
{
    var existing = await ctx.EvaluationCriteria.FirstOrDefaultAsync(c => c.Name == name);
    if (existing != null) return existing.Id;

    var entity = new EvaluationCriteria
    {
        Id           = Guid.NewGuid(),
        Name         = name,
        Description  = description,
        Instructions = instructions,
        Subject      = subject,
        IsActive     = true,
        CreatedAt    = DateTime.UtcNow
    };
    ctx.EvaluationCriteria.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task UpsertQuestion(InsyteDbContext ctx, Guid criteriaId,
    string question, string category, int order)
{
    var exists = await ctx.EvaluationQuestions.AnyAsync(q =>
        q.CriteriaId == criteriaId && q.Question == question);
    if (exists) return;

    ctx.EvaluationQuestions.Add(new EvaluationQuestion
    {
        Id         = Guid.NewGuid(),
        CriteriaId = criteriaId,
        Question   = question,
        Category   = category,
        Order      = order,
        IsActive   = true,
        CreatedAt  = DateTime.UtcNow
    });
    await ctx.SaveChangesAsync();
}

static async Task<Guid> UpsertClass(InsyteDbContext ctx, Guid schoolId,
    string name, ClassLevel level, string type)
{
    var existing = await ctx.Classes.FirstOrDefaultAsync(c =>
        c.SchoolId == schoolId && c.Name == name);
    if (existing != null) return existing.Id;

    var entity = new Class
    {
        Id        = Guid.NewGuid(),
        SchoolId  = schoolId,
        Name      = name,
        Level     = level,
        Type      = type,
        IsActive  = true,
        CreatedAt = DateTime.UtcNow
    };
    ctx.Classes.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task<Guid> UpsertSubject(InsyteDbContext ctx, Guid schoolId,
    string name, string branch, string level, int weeklyHours)
{
    var existing = await ctx.Subjects.FirstOrDefaultAsync(s =>
        s.SchoolId == schoolId && s.Name == name);
    if (existing != null) return existing.Id;

    var entity = new Subject
    {
        Id          = Guid.NewGuid(),
        SchoolId    = schoolId,
        Name        = name,
        Branch      = branch,
        Level       = level,
        WeeklyHours = weeklyHours,
        IsActive    = true,
        CreatedAt   = DateTime.UtcNow
    };
    ctx.Subjects.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task<Guid> UpsertVideo(InsyteDbContext ctx, string title,
    string filePath, string? originalFileName, long fileSize,
    Guid schoolId, Guid teacherId, Guid uploadedById,
    string? subject, VideoStatus status)
{
    var existing = await ctx.Videos.FirstOrDefaultAsync(v => v.FilePath == filePath);
    if (existing != null) return existing.Id;

    var entity = new Video
    {
        Id               = Guid.NewGuid(),
        Title            = title,
        FilePath         = filePath,
        OriginalFileName = originalFileName,
        FileSize         = fileSize,
        SchoolId         = schoolId,
        TeacherUserId    = teacherId,
        UploadedByUserId = uploadedById,
        Subject          = subject,
        Status           = status,
        CreatedAt        = DateTime.UtcNow.AddDays(-new Random().Next(1, 30))
    };
    ctx.Videos.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task<Guid> UpsertEvaluation(InsyteDbContext ctx,
    Guid videoId, Guid criteriaId, Guid aiModelId,
    EvaluationStatus status, string? result,
    int inputTokens, int outputTokens)
{
    var existing = await ctx.Evaluations.FirstOrDefaultAsync(e => e.VideoId == videoId);
    if (existing != null) return existing.Id;

    var entity = new Evaluation
    {
        Id               = Guid.NewGuid(),
        VideoId          = videoId,
        CriteriaId       = criteriaId,
        AIModelId        = aiModelId,
        Result           = result,
        TokenUsageInput  = inputTokens,
        TokenUsageOutput = outputTokens,
        Status           = status,
        CreatedAt        = DateTime.UtcNow.AddDays(-new Random().Next(1, 20)),
        CompletedAt      = status == EvaluationStatus.Completed ? DateTime.UtcNow.AddDays(-new Random().Next(0, 5)) : null
    };
    ctx.Evaluations.Add(entity);
    await ctx.SaveChangesAsync();
    return entity.Id;
}

static async Task UpsertReport(InsyteDbContext ctx,
    Guid evaluationId, Guid? approvedById, DateTime? approvedAt,
    ReportStatus status, string? pdfPath = null)
{
    var exists = await ctx.Reports.AnyAsync(r => r.EvaluationId == evaluationId);
    if (exists) return;

    ctx.Reports.Add(new Report
    {
        Id              = Guid.NewGuid(),
        EvaluationId    = evaluationId,
        PdfPath         = pdfPath,
        ApprovedByUserId= approvedById,
        ApprovedAt      = approvedAt,
        Status          = status,
        CreatedAt       = DateTime.UtcNow.AddDays(-new Random().Next(0, 15))
    });
    await ctx.SaveChangesAsync();
}
