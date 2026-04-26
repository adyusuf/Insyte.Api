# Insyte Seed Verisi

Geliştirme ve test ortamı için örnek veri ve placeholder görseller.

## Çalıştırma

```bash
cd api
dotnet script SeedDb.cs
```

> Bağlantı bilgisi `SeedDb.cs` içindeki connection string'e göre belirlenir.
> Üretim ortamında ÇALIŞTIRILMAZ.

## Oluşturulan Veriler

| Kategori         | Adet | Detay                                              |
|------------------|------|----------------------------------------------------|
| Kullanıcılar     | 19   | 1 admin, 3 danışman, 3 okul yöneticisi, 12 öğretmen |
| Okullar          | 3    | Ankara, İstanbul, İzmir                            |
| Danışman ataması | 6    | Her okula 1-2 danışman                             |
| Okul kullanıcısı | 15   | Yönetici + öğretmenler                             |
| AI Sağlayıcı     | 3    | OpenAI, Anthropic, Google AI                       |
| AI Model         | 5    | gpt-4o, gpt-4o-mini, claude-sonnet-4-5, claude-haiku, gemini-2.5-pro |
| Kriter           | 3    | Genel, Matematik, Sınıf Yönetimi                   |
| Kriter Sorusu    | 18   | Her kritere 5-8 soru                               |
| Sınıf            | 11   | 3 okulda toplam                                    |
| Ders             | 11   | 3 okulda toplam                                    |
| Video            | 7    | Farklı durumlar: Uploaded, Evaluated, Approved     |
| Değerlendirme    | 5    | Tümü Completed, gerçekçi AI yanıt JSON'ları        |
| Rapor            | 5    | 2 Draft, 1 Approved, 2 Sent                        |

## Kullanıcı Bilgileri

```
admin@insyte.com              Admin@123     (Admin)
mehmet.yilmaz@insyte.com      Danisman@123  (Danışman)
selin.arslan@insyte.com       Danisman@123  (Danışman)
kerem.ozturk@insyte.com       Danisman@123  (Danışman)
yonetici@anadolu.k12.tr       Yonetici@123  (Okul Yöneticisi - Ankara)
yonetici@bosporus.k12.tr      Yonetici@123  (Okul Yöneticisi - İstanbul)
yonetici@izmir.k12.tr         Yonetici@123  (Okul Yöneticisi - İzmir)
ali.demir@anadolu.k12.tr      Ogretmen@123  (Öğretmen - Matematik)
fatma.sahin@anadolu.k12.tr    Ogretmen@123  (Öğretmen - Fizik)
...diğer 10 öğretmen          Ogretmen@123
```

## Görseller

Placeholder görseller `images/` altında SVG formatında:

```
seed/images/
├── okul-logo/
│   ├── anadolu-fen-lisesi.svg
│   ├── bosporus-ozel-lisesi.svg
│   └── izmir-ege-ilkokulu.svg
├── ogretmen/          ← 12 öğretmen avatarı (baş harfli)
│   ├── ali-demir.svg
│   ├── fatma-sahin.svg
│   └── ...
└── profil/            ← 7 admin/danışman/yönetici avatarı
    ├── admin.svg
    ├── mehmet-yilmaz.svg
    └── ...
```

> **Not:** Gerçek ortamda bu görseller AI ile üretilir (Stable Diffusion, DALL-E 3 vb.).
> Geliştirme için placeholder SVG'ler yeterlidir.
> Detay: `docs/seed-data.md`
