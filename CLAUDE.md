# API — .NET 8 + PostgreSQL

> Bu dosya `api/` klasöründe çalışırken Claude Code'un okuduğu rehberdir.
> Kök kurallar `../CLAUDE.md`'de. Burası API'ye özel.

## Stack

- **.NET 8** (LTS) — Controller tabanlı yapı
- **PostgreSQL 16+**
- **Entity Framework Core 8** — Npgsql provider
- **FluentValidation** — request validasyonu
- **Serilog** — yapılandırılmış log
- **Mapster** — DTO eşlemeleri
- **xUnit** + **FluentAssertions** + **Testcontainers** — test
- **Hangfire** — AI iş kuyruğu, PDF üretimi, e-posta gönderimi (background jobs)
- **QuestPDF** — PDF rapor üretimi (Türkçe karakter dostu, MIT lisanslı)
- **MailKit** — SMTP (SendGrid/Postmark da değerlendirilebilir)
- **Azure.Storage.Blobs** veya **AWSSDK.S3** veya **Minio** — video dosya storage
- **FFMpegCore** — video metadata çıkarma, gerekirse transcode/sıkıştırma
- **Polly** — AI çağrılarında retry, circuit breaker
- **Bogus** — seed data
- Auth: **JWT bearer**, refresh token desteği

## Klasör yapısı

```
api/
├── src/
│   ├── Insyte.Api/                  ← Controllers, middleware, Program.cs
│   ├── Insyte.Application/          ← Use cases, services, DTO, validators
│   ├── Insyte.Domain/               ← Entity'ler, enum'lar, domain kuralları
│   ├── Insyte.Infrastructure/       ← EF DbContext, repositories, storage
│   ├── Insyte.AI/                   ← AI sağlayıcı abstractionı + adaptörler
│   │   ├── Abstractions/
│   │   │   └── IAiSaglayici.cs
│   │   ├── Providers/
│   │   │   ├── OpenAiSaglayici.cs
│   │   │   ├── AnthropicSaglayici.cs
│   │   │   ├── GoogleSaglayici.cs
│   │   │   └── CustomHttpSaglayici.cs
│   │   └── PromptInsa.cs
│   ├── Insyte.Pdf/                  ← QuestPDF rapor şablonları
│   └── Insyte.Mail/                 ← E-posta gönderimi + şablonları
├── tests/
│   ├── Insyte.UnitTests/
│   └── Insyte.IntegrationTests/
├── seed/
└── docs/
    ├── api-konvansiyon.md
    ├── auth.md
    ├── yetkilendirme-teknik.md
    ├── ai-entegrasyon.md            ← AI sağlayıcı abstraction detayı
    ├── pdf-uretimi.md
    ├── eposta-pipeline.md
    ├── video-storage.md
    ├── migration-rehberi.md
    └── hata-kodlari.md
```

**Katman bağımlılığı:**
`Api → Application → Domain`
`Infrastructure / AI / Pdf / Mail → Application + Domain`
Domain hiçbir şeye bağlı değildir.

## API konvansiyonları

### URL ve HTTP

- Endpoint kökü: `/api/v1/...`
- Çoğul kaynak adı: `/api/v1/okullar`, `/api/v1/ogretmenler`, `/api/v1/raporlar`
- HTTP fiilleri standart
- Liste endpoint'leri **paginated**: `?page=1&pageSize=20&sort=-createdAt`
- Filtre query string: `?okulId=...&durum=onayBekliyor`

### Yanıt formatı

Başarılı:
```json
{
  "data": { ... },
  "meta": { "page": 1, "pageSize": 20, "total": 142 }
}
```

Hata:
```json
{
  "error": {
    "code": "VIDEO_BULUNAMADI",
    "message": "Belirtilen ID ile video bulunamadı.",
    "details": []
  }
}
```

Hata kodları kataloğu: `docs/hata-kodlari.md`.

### HTTP durum kodları

- `200 OK` — başarılı GET/PUT
- `201 Created` — başarılı POST + `Location` header
- `204 No Content` — başarılı DELETE
- `400 Bad Request` — validasyon hatası
- `401 Unauthorized` — token yok/geçersiz
- `403 Forbidden` — yetki yok (okul-bazlı + sayfa-bazlı kontrol!)
- `404 Not Found` — kaynak yok
- `409 Conflict` — iş kuralı çakışması
- `422 Unprocessable Entity` — domain kuralı ihlali
- `429 Too Many Requests` — rate limit / token kotası
- `500` — beklenmeyen hata

## Veritabanı kuralları

- **Tablo isimleri snake_case + çoğul:** `okullar`, `ogretmenler`, `videolar`, `raporlar`
- **Sütunlar snake_case:** `created_at`, `onay_durumu`
- **PK:** `id` (uuid v7)
- **Her tabloda:** `created_at`, `updated_at`, `deleted_at` (soft delete) — UTC
- **FK:** `<tekil>_id` formatında (`okul_id`, `ogretmen_id`)
- **Index'ler migration'da açıkça tanımlanır.** Multi-tenant sorgu için `(okul_id, ...)` index kritik.
- **JSONB kullan:** AI sonucu, kriter detayları gibi esnek alanlarda.
- **Tenant scope:** Her sorgu `okul_id` filtresi veya kullanıcının erişim listesinden geçer. Global query'ler sadece admin için.
- **Migration'lar geri alınabilir.**
- **Veriyi silen migration yazılmaz.**

## Auth

- Şifre hash: **Argon2id**
- Access token: **15 dakika**
- Refresh token: **30 gün**, DB'de tutulur, revoke edilebilir
- Token claim'leri: `sub` (user_id), `email`, `tip` (admin|advisor|okul_yoneticisi), `roller` (string[]), `okullar` (uuid[])
- Şifre sıfırlama tek kullanımlık + 1 saat geçerli
- Kullanıcı oluşturma sadece admin/yetkili tarafından — public registration yok

Detaylar: `docs/auth.md`.

## Yetkilendirme — kritik

Çift-eksenli model:

1. **Sayfa/aksiyon yetkisi** — "videoları listele", "rapor onayla", "AI tanımı düzenle"
2. **Okul kapsamı** — bu kullanıcı hangi okullarda yetkili?

İki kontrol birden geçilmeli:

```csharp
[HttpPost("/api/v1/raporlar/{id}/onayla")]
public async Task<IActionResult> RaporOnayla(Guid id)
{
    var rapor = await _raporlar.GetirAsync(id);
    if (rapor is null) return NotFound();

    // 1. Sayfa-bazlı yetki
    if (!await _yetki.AksiyonYapabilirMi(KullaniciId, "rapor.onayla"))
        return Forbid();

    // 2. Okul-bazlı yetki
    if (!await _yetki.OkulErisimi(KullaniciId, rapor.OkulId))
        return Forbid();

    // ...
}
```

Yetkilendirme matrisi: `../docs/yetkilendirme.md`. Teknik: `docs/yetkilendirme-teknik.md`.

## Çalışma Grupları & Kurullar

**Endpoint'ler:**
- `GET /api/v1/working-groups` — liste (search + pagination)
- `POST /api/v1/working-groups` — oluştur
- `GET /api/v1/working-groups/{id}` — detay
- `PUT /api/v1/working-groups/{id}` — güncelle
- `DELETE /api/v1/working-groups/{id}` — soft delete
- `GET /api/v1/working-groups/{id}/members` — üye listesi
- `POST /api/v1/working-groups/{id}/members` — üye ekle
- `DELETE /api/v1/working-groups/{id}/members/{memberId}` — üye sil
- `PUT /api/v1/working-groups/{id}/members/{memberId}` — üye rolü güncelle

**Aynı endpoints `/api/v1/councils` için de geçerli.**

**Özellikler:**
- Okul-bazlı scope: her sorgu kullanıcının erişim listeleri filtrelemeleri
- Policy-based yetki: `AllStaff` (oluştur/güncelle), `AdminOnly` (sil)
- Üyeler `WorkingGroupMember` / `CouncilMember` tabloları üzerinden
- Token ve transactional integrity
- Soft delete pattern

## AI orkestrasyonu

**Akış:**

1. Kullanıcı video yükler → DB'ye kayıt + dosya storage'a (`durum=yuklendi`).
2. Kullanıcı kriter seçer + opsiyonel düzenleme yapar.
3. Kullanıcı **"İncelemeye gönder"** butonuna basar (otomatik değil).
4. Backend Hangfire job kuyruğuna ekler (`durum=kuyrukta`).
5. Worker:
   - Aktif AI sağlayıcı + model alınır (sistem ayarından)
   - Kriter prompt'u + video referansı (URL veya dosya) ile sağlayıcıya gönderilir
   - Yanıt + token sayısı alınır
   - JSON parse edilir, `Rapor` kaydı oluşturulur (`durum=onay_bekliyor`)
   - Token bakiyesi düşülür
6. Yönetici onaylar (`durum=onaylandi`) → PDF üretilir → e-posta dağıtım listesine gider.

**Sıralı işleme:** AI çağrıları **sırayla** yapılır (paralel değil) — istisnasız. Aynı sağlayıcıdaki rate limit + maliyet kontrolü için Hangfire kuyruğu seri çalışır.

**Hata yönetimi:**

- AI hatası (timeout, 5xx) → Polly retry (max 3 deneme, exponential backoff)
- Sürekli başarısız → `durum=basarisiz`, hata mesajı kayıt, kullanıcıya bildirim
- Token kotası dolarsa → `durum=basarisiz_kota`

**Sağlayıcı bağımsızlığı:** Tüm sağlayıcılar `IAiSaglayici` arayüzü üzerinden konuşur:

```csharp
public interface IAiSaglayici
{
    Task<AiYanit> AnalizEtAsync(AiIstek istek, CancellationToken ct);
}

public record AiIstek(
    string ModelAdi,
    string SistemPrompt,
    string KullaniciPrompt,
    Uri VideoUrl,
    string YanitFormati);   // JSON schema

public record AiYanit(
    string YanitJson,
    int InputToken,
    int OutputToken,
    int ToplamToken,
    TimeSpan Sure);
```

Detay: `docs/ai-entegrasyon.md`.

## Video storage

- Dosyalar **DB'de tutulmaz**. Object storage (S3-uyumlu).
- Yükleme: presigned URL ile direkt client → storage. Backend dosyayı görmez.
- Backend metadata kayıt eder + AI'a gönderirken signed URL üretir.
- Boyut limiti: max **2GB** per video (yapılandırılabilir).
- Format kontrolü: mp4, mov, webm, mkv (FFMpeg ile metadata doğrulama).
- Detay: `docs/video-storage.md`.

## PDF üretimi

- **QuestPDF** kullanılır (Türkçe karakterler, kolay layout, MIT).
- Şablon: kapak, özet, kriter bazlı detaylar, video bilgileri, footer.
- Stilistik: marka rengi + Inter font.
- PDF storage: aynı bucket, `raporlar/<id>.pdf`.
- Üretim Hangfire job (büyük PDF'ler request thread'ini bloklamaz).
- Detay: `docs/pdf-uretimi.md`.

## E-posta dağıtımı

- Onaylanan rapor → ilgili okulun **dağıtım listesi**ne otomatik e-posta.
- Şablon: HTML + plain text fallback. Türkçe.
- PDF ek olarak iliştirilir.
- Gönderim Hangfire job, retry'lı.
- Bounce/failure log'lanır, dashboard'da görünür.
- Detay: `docs/eposta-pipeline.md`.

## Validasyon

- **FluentValidation** — controller'da manuel `if` ile validasyon yok
- Her DTO için `<DTO>Validator` sınıfı
- Hata mesajları **Türkçe**
- Domain kuralı (örn: "video boyutu 2GB'ı aşamaz") Domain entity'sinde

## Test

- **Unit:** Application service'leri, validator, mapper, Domain kuralı
- **Integration:** API endpoint'leri — Testcontainers ile gerçek PostgreSQL
- **AI sağlayıcılar mock'lanır** integration testte (gerçek API çağrısı yapılmaz)
- **Test isimlendirme:** `MetodAdi_Senaryo_BeklenenSonuc`
- Coverage: Application %80+, Domain %90+
- Tüm testler `dotnet test` ile çalışır

## Loglama

- **Serilog** + structured logging
- AI çağrılarında: prompt, model, token, süre, hata (varsa) kaydedilir — **API anahtarı asla**
- **Asla loglanmaz:** Şifreler, JWT, refresh token, AI API anahtarı, SMTP şifresi
- Correlation ID her request'te

## Performans

- Akış sorgusu (rapor listesi okul bazında) index gerektirir
- AI worker: tek worker yeterli başlangıçta (sıralı işleme); ölçek olduğunda multi-worker + sağlayıcı bazında kuyruk ayrımı
- 100+ kayıt dönen endpoint pagination zorunlu
- N+1 query yasak
- Cache: Memory cache (kategori listesi, AI tanımları gibi sık okunan, az değişen)

## Geliştirme akışı

```bash
dotnet restore
dotnet ef migrations add <ad> --project src/Insyte.Infrastructure --startup-project src/Insyte.Api
dotnet ef database update --project src/Insyte.Infrastructure --startup-project src/Insyte.Api
dotnet run --project seed/SeedRunner
dotnet run --project src/Insyte.Api
dotnet test
```

`.env` örneği:
```
ConnectionStrings__Default=Host=localhost;Database=insyte;Username=...;Password=...
Jwt__SecretKey=...
Storage__Provider=S3
Storage__Bucket=insyte-videos
Storage__AccessKey=...
Storage__SecretKey=...
Smtp__Host=...
Smtp__Username=...
Smtp__Password=...
```

**AI sağlayıcı anahtarları DB'de şifreli — environment'ta tutulmaz.**

## Yapma

- ❌ Controller içine iş mantığı yazma
- ❌ Entity'leri doğrudan controller'dan döndürme
- ❌ Yetki kontrolünü unutma. **Hem sayfa hem okul** kontrol edilir.
- ❌ AI çağrısını sync yapma — Hangfire job
- ❌ AI yanıtını kullanıcıya **otomatik göstermek** — onay süreci kritik
- ❌ Token sayısını kayıt etmemek — her çağrıda kayıt zorunlu
- ❌ Video dosyasını DB'de tutmak
- ❌ AI API anahtarını düz metin saklamak
- ❌ Production'da `Database.EnsureCreated()`
- ❌ `async void` (event handler hariç)
- ❌ `.Result` / `.Wait()`
