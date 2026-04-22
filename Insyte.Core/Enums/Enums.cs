namespace Insyte.Core.Enums;

public enum UserRole
{
    Admin,
    Advisor,
    SchoolAdmin,
    Teacher
}

public enum VideoStatus
{
    Uploaded,
    Processing,
    Evaluated,
    Approved,
    Rejected
}

public enum EvaluationStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}

public enum ReportStatus
{
    Draft,
    Approved,
    Sent
}

public enum RecipientType
{
    Principal,
    Teacher,
    Advisor,
    Other
}

public enum EmailStatus
{
    Pending,
    Sent,
    Failed
}

public enum SchoolType
{
    Anaokulu,         // Kindergarten
    Ilkokul,          // Primary School
    Ortaokul,         // Middle School
    Lise,             // High School
    UniversitePrepare,// University Prep
    Universitesi,     // University
    Meslek            // Vocational
}

public enum LiseType
{
    AnadoluMeslekLisesi,              // Anadolu Vocational High School
    AnadoluLisesi,                    // Anadolu High School
    SosyalBilimleriLisesi,            // Social Sciences High School
    FenLisesi,                        // Science High School
    GuzelSanatlarLisesi,              // Fine Arts High School
    CokProgramliAnadoluLisesi,        // Multi-Program Anadolu High School
    AnadoluImamHatipLisesi,           // Anadolu Imam Hatip High School
    SporLisesi,                       // Sports High School
    AksamLisesi,                      // Evening High School
    FenVeTeknolojiLisesi,             // Science and Technology High School
    MeslekiVeTeknikAnadoluLise        // Vocational and Technical Anadolu High School
}

public enum EducationSystem
{
    MEBOdakliEgitim,                  // MEB-Focused Education
    KlasikSistem,                     // Classical System
    EklektikYaklas,                   // Eclectic Approach
    CokluZekaModeli,                  // Multiple Intelligences Model
    UBD,                              // Understanding by Design
    PYP,                              // Primary Years Programme
    CommonCoreStateStandards,         // Common Core State Standards
    APlusCinqBEgitim,                 // a+5b Education Model
    WaldorfPedagojisi,                // Waldorf Pedagogy
    STEM,                             // STEM
    AktifOgrenmeSistemi,              // Active Learning System
    HibritEgitimModeli,               // Hybrid Education Model
    MasalTerapisiEgitimi,             // Fairy Tale Therapy Education
    AfayTekniği,                      // Afay Technique
    MYP,                              // Middle Years Programme
    DP,                               // Diploma Programme
    AileCalismaSosyalHizmetlerBakanligi,  // Ministry of Family, Labor and Social Services Education
    GölgeEgitimSistemi,               // Shadow Education System
    ProjeTemelliOgrenme,              // Project-Based Learning
    SinavOdakliEgitim,                // Exam-Focused Education
    IsbirlikliOgrenmeModeli,          // Collaborative Learning Model
    YaparakYasayarakOgrenme,          // Learning by Doing
    OgrenciMerkezliEgitimModeli,      // Student-Centered Education Model
    BilingualEgitim,                  // Bilingual Education
    InternationalBaccalaureate,       // International Baccalaureate (IB)
    CambridgeEgitimSistemi,           // Cambridge Education System
    TamZamanliIngilizceEgitimi,       // Full-Time English Education
    EkolojiTemelliBgitim,             // Ecology-Based Education
    IGCSE,                            // International General Certificate of Secondary Education
    TamOgrenmeModeli,                 // Holistic Learning Model
    HolistikEgitim,                   // Holistic Education
    KariyerGelistimProgrami,          // Career Development Program
    BeceriYetkinlikGelistimProgrami,  // Skills Competency Development Program
    AdvancedPlacementProgram,         // Advanced Placement (AP) Program
    SATProgram,                       // SAT (Scholastic Assessment Test)
    UCASProgram                       // UCAS (Universities and Colleges Admissions Service)
}

public enum PhysicalFacility
{
    UykuOdasi,                        // Dormitory
    Yemekhane,                        // Dining Hall
    Havuz,                            // Swimming Pool
    BilgisayarLaboratorivari,         // Computer Lab
    KapalISporSalonu,                 // Indoor Sports Hall
    FutbolSahasi,                     // Football Field
    KonferansSalonu,                  // Conference Hall
    Laboratorivari,                   // Laboratory
    SanatAtoliyesi,                   // Art Workshop
    Kantin,                           // Cafeteria
    Kutupahane,                       // Library
    MuzikOdasi,                       // Music Room
    OyunAlani,                        // Play Ground
    Revir,                            // Health Center
    Bahce,                            // Garden
    Lojman,                           // Staff Housing
    AkillTahta,                       // Smart Board
    HayvanatBahcesi,                  // Zoo
    Sera,                             // Greenhouse
    IcBoyutluOdasi,                   // 3D Room
    MutfakAtoliyesi,                  // Cooking Workshop
    SporAlani,                        // Sports Field
    KumHavuzu                         // Sand Pool
}

public enum SchoolService
{
    Guvenlik,                         // Security
    Rehberlik,                        // Guidance
    YazOkulu,                         // Summer School
    Servis,                           // School Bus
    HaftasonuEgitim,                  // Weekend Education
    OrganikBeslenme,                  // Organic Nutrition
    OyunGrubu,                        // Play Group
    AnneCocukOyunGrubu,               // Mother-Child Play Group
    DiniEgitim                        // Religious Education
}

public enum Activity
{
    Futbol,                           // Football
    Voleybol,                         // Volleyball
    Basketbol,                        // Basketball
    Judo,                             // Judo
    MasaTenisi,                       // Table Tennis
    SuTopu,                           // Water Polo
    Fotografcilik,                    // Photography
    Satranc,                          // Chess
    Yuzme,                            // Swimming
    Seramik,                          // Ceramics
    Bale,                             // Ballet
    Origami,                          // Origami
    Hentbol,                          // Handball
    Sinema,                           // Cinema
    SuBalesi,                         // Water Ballet
    DekoratifSanatlar,                // Decorative Arts
    YabancıDilKlubu,                  // Foreign Language Club
    Heykel,                           // Sculpture
    Muzik,                            // Music
    ModernDans,                       // Modern Dance
    Tenis,                            // Tennis
    Drama,                            // Drama
    GoruselSanatlar,                  // Visual Arts
    Tiyatro,                          // Theatre
    Eskrim,                           // Fencing
    Gures,                            // Wrestling
    Ebru,                             // Marbling
    Izcilik,                          // Scouting
    Atletizm,                         // Athletics
    Binicilik,                        // Horseback Riding
    Okculuk,                          // Archery
    Proje,                            // Project
    BuzPateni,                        // Ice Skating
    Gezi,                             // Excursion
    Tirmanis,                         // Climbing
    Perkusyon,                        // Percussion
    Piyano,                           // Piano
    IngilizzeDrama,                   // English Drama
    BilisimKlubu,                     // IT Club
    Gazetecilik,                      // Journalism
    Orkestra,                         // Orchestra
    Koro,                             // Choir
    Jimnastik,                        // Gymnastics
    Ekoloji,                          // Ecology
    HalkOyunlari,                     // Folk Dances
    Dans,                             // Dance
    AkilVeZekaOyunlari,               // Mind and Logic Games
    Yoga,                             // Yoga
    ElSanatlari,                      // Handicrafts
    DegerlerEgitimi,                  // Values Education
    Orff,                             // Orff
    PlanetariumGokBilimi,             // Planetarium - Astronomy
    Robotik,                          // Robotics
    Badminton,                        // Badminton
    BedEnEgitimi,                     // Physical Education
    Taekwondo,                        // Taekwondo
    Gitar,                            // Guitar
    Karate,                           // Karate
    Pilates,                          // Pilates
    ESpor,                            // E-Sports
    Kodlama                           // Coding
}

public enum ForeignLanguage
{
    Ingilizce,                        // English
    Almanca,                          // German
    Francizca,                        // French
    Ispanyolca,                       // Spanish
    Italyanca,                        // Italian
    Cinece,                           // Chinese
    Ruscaj,                           // Russian
    Arapca,                           // Arabic
    Japonica,                         // Japanese
    Ibrahimce,                        // Hebrew
    Ermenice,                         // Armenian
    Ukraynaca                         // Ukrainian
}

public enum InstitutionType
{
    Devlet,           // Public
    Ozel,             // Private
    Vakif             // Foundation
}

public enum ClassLevel
{
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6,
    Level7,
    Level8,
    Level9,
    Level10,
    Level11,
    Level12,
    Other
}

public enum ScheduleDay
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
