using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluationCriteriaAndQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EvaluationQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CriteriaId = table.Column<Guid>(type: "uuid", nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluationQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EvaluationQuestions_EvaluationCriteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "EvaluationCriteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EvaluationCriteria",
                columns: new[] { "Id", "CreatedAt", "Description", "Instructions", "IsActive", "Name", "Subject", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dersin yapısı, hedef belirleme, müfredatla uyum, konuyu doğru aktarma", "Öğretmenin ders planını oluşturma, hedefler belirleme ve müfredata uygun içerik sunma becerisini değerlendirin.", true, "Pedagojik Yeterlilik", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ses tonu, konuşma hızı, beden dili, göz teması, dil kullanımı", "Öğretmenin iletişim stilini, konuşma netliğini, beden dilini ve sınıf içinde etkileşim kurma becerilerini değerlendirin.", true, "İletişim & Sunum Becerileri", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Öğrencileri derse dahil etme, sorular sorma, ilgiyi canlı tutma", "Öğrencilerin derse katılım düzeyini, motivasyon seviyesini ve öğretmenin etkileşim kurma çabalarını değerlendirin.", true, "Öğrenci Katılımı & Motivasyonu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Düzeni sağlama, zaman yönetimi, disiplin yaklaşımı", "Sınıf düzeninin sağlanması, zamanın etkili kullanılması ve disiplin yaklaşımının uygunluğunu değerlendirin.", true, "Sınıf Yönetimi", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Öğrenci sorularını yanıtlama, anlık geri bildirim, yönlendirme", "Öğrenci sorularına verilen cevapların kalitesini, geri bildirim etkinliğini ve yapıcı yönlendirmeyi değerlendirin.", true, "Soru-Cevap & Geri Bildirim Kalitesi", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tahta, slayt, araç-gereç kullanımı, materyallerin etkinliği", "Öğretim materyallerinin uygunluğunu, çeşitliliğini ve etkili kullanımını değerlendirin.", true, "Materyal & Görsel Kullanımı", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Farklı öğrenme düzeylerine hitap etme, bireysel ilgi", "Öğretmenin farklı öğrenme stillerine ve seviyelere uygun öğretim stratejileri kullanıp kullanmadığını değerlendirin.", true, "Farklılaştırılmış Öğretim", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Öğrencilerin ilgi düzeyi, katılım, beden dili, motivasyon", "Öğrencilerin derste gösterdikleri ilgi, katılım düzeyi, beden dilini ve genel motivasyon durumunu değerlendirin.", true, "Öğrenci Davranışı & Tepkileri", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Konuyu doğru ve anlaşılır aktarma, yanlış bilgi var mı", "Öğretmenin konuyu doğru bir şekilde anlatan, yanlış bilgi içermeyen ve anlaşılır bir dil kullanıp kullanmadığını değerlendirin.", true, "Dil & Kavram Doğruluğu", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000010"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Başlangıç-orta-kapanış dengesi, geçişler, süre yönetimi", "Dersin başlangıcından sonuna kadar akışını, geçişlerin uygunluğunu ve zamanın dengeli kullanılıp kullanılmadığını değerlendirin.", true, "Genel Ders Akışı & Tempo", null, null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$2PfOtcqfnVBIePYTn4cJUejLkv0CFekraRflHRPg6mahWjkQRlHt.");

            migrationBuilder.InsertData(
                table: "EvaluationQuestions",
                columns: new[] { "Id", "Category", "CreatedAt", "CriteriaId", "IsActive", "Order", "Question", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0382c84e-b82c-490f-b626-a794fe98600b"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 41, "Kullanılan materyaller (slayt, tahta, video, vs.) içerikle uyumlu ve kaliteli mi?", null },
                    { new Guid("0823e929-60b9-4afc-8967-3ed2253f4300"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 9, "Göz teması kuruyor mu — sadece tahtaya mı bakıyor, öğrencilere mi?", null },
                    { new Guid("0b1ab1f9-372f-4b33-8c87-a24014840883"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 38, "Zamanı iyi yönetiyor mu — ders planladığı gibi ilerliyor mu?", null },
                    { new Guid("0d160b27-865d-4134-91ec-c52704c29f36"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 31, "Öğrencinin sorusunu doğru anlayıp yanıtlıyor mu?", null },
                    { new Guid("0dbf2d51-1dfa-4557-99f7-f613a4ad19d4"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 48, "Öğrencilerin soru sorma ya da katılım istekliliği var mı?", null },
                    { new Guid("12c2d448-e9fd-433a-8601-6d5f235786e9"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 2, "Konuşma hızı uygun mu — çok hızlı veya çok yavaş mı?", null },
                    { new Guid("1549b195-3c01-45d9-9c98-efb2b696a8fb"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 34, "Öğrencilerin anladığını nasıl kontrol ediyor — anlama soruları soruyor mu?", null },
                    { new Guid("1570409a-824d-42a1-827e-57fd81c49398"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 5, "Türkçeyi (ya da ders dilini) doğru ve akıcı kullanıyor mu?", null },
                    { new Guid("1ba857aa-949d-463e-81a6-162e1a2eb927"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 6, "Gereksiz dolgu sözler (yani, şey, hmm) ne sıklıkla kullanılıyor?", null },
                    { new Guid("1cd04f93-bec7-4884-ab21-0c68c68c1650"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 56, "Bu dersi izleyen biri ne öğrendiğini açıklayabilir mi?", null },
                    { new Guid("1ded10a3-817a-438a-b6fe-6fa97b7242dd"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 19, "Ders sonunda özet ya da tekrar yapıyor mu?", null },
                    { new Guid("20b53b68-42eb-4c2a-a38e-333c7a0381bd"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 7, "Öğrencilere hitap ederken ne kadar samimi ve sıcak bir ton kullanıyor?", null },
                    { new Guid("21fe0584-e49d-4a9c-88a3-7a1b5a79363f"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 47, "Öğrenciler ders sırasında ne yapıyor — not mu alıyor, telefonla mı ilgileniyor, konuşuyor mu?", null },
                    { new Guid("3f3084c9-3ba8-4987-b6eb-4da114ce9c80"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 25, "Öğrencilerin birbirini dinlemesini ve tartışmasını teşvik ediyor mu?", null },
                    { new Guid("4285c477-f0eb-4130-83de-aa636b738b7e"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 18, "Önceki derslerle bağlantı kuruyor mu?", null },
                    { new Guid("4e342c4d-3016-4478-b8fa-762880ef19f6"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 29, "Öğrenci cevapları doğru olduğunda nasıl geri bildirim veriyor?", null },
                    { new Guid("5219b2d1-117a-46f0-9f12-5d8bed792945"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 58, "Öğretmenin en güçlü 3 yönü nedir?", null },
                    { new Guid("52f9c5da-aaff-47b3-97b8-7a4f752ae100"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 61, "Bir sonraki derste öncelikli olarak iyileştirilmesi gereken tek şey ne?", null },
                    { new Guid("5d05a810-f146-4d19-9a1d-8375d612fbe8"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 52, "Dersin hangi anlarında öğrenci enerjisi yükseliyor ya da düşüyor?", null },
                    { new Guid("602f1645-eb12-4ca5-a8ec-6c266f69e001"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 22, "Soruları sadece belli öğrencilere mi, yoksa tüm sınıfa mı yöneltiyor?", null },
                    { new Guid("60c6b183-3c5d-4dd2-bf83-ef2c8657313e"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 44, "Teknolojiyi etkin mi kullanıyor?", null },
                    { new Guid("6d5e6acf-e597-4dc7-b962-5d9bb90f2881"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 39, "Geçişler (yeni konuya geçiş, aktivite değişimi) pürüzsüz mü?", null },
                    { new Guid("6d7690eb-a98d-4bc3-b717-709328d911de"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 53, "Farklı öğrenme hızlarındaki öğrencilere uyum sağlıyor mu?", null },
                    { new Guid("70736b3f-672f-474c-9a7c-fae3301642ba"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 14, "Konu anlatımı mantıksal bir sırayla ilerliyor mu?", null },
                    { new Guid("72153382-9a12-468e-87a6-6967181c04e5"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 15, "Öğrettiği bilgiler doğru mu — kavram veya bilgi hatası var mı?", null },
                    { new Guid("7725d37b-87a8-42db-a426-6f6579506336"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 35, "Sınıfta genel düzen ve sessizlik sağlanabiliyor mu?", null },
                    { new Guid("795e3b40-a8f2-4b2e-aab2-4d5e5de32ce9"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 13, "Dersin başında öğrencilere neyi öğreneceklerini açıklıyor mu?", null },
                    { new Guid("7bc3a583-cad7-4eab-9223-c58acb5f4bba"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 8, "Öğretmen sınıfı aktif olarak dolaşıyor mu, yoksa tek noktada mı duruyor?", null },
                    { new Guid("7bfa2df8-e0de-491d-833d-ae1edaf36933"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 49, "Sınıfta pozitif, güvenli bir öğrenme ortamı var mı?", null },
                    { new Guid("7cc01e21-d27d-4ec8-8369-66dba9d50175"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 51, "Öğrencilerin yüz ifadeleri ve beden dili neler söylüyor — sıkılmış mı, meraklı mı?", null },
                    { new Guid("7e3963b7-eda3-4415-9b7e-b11a7a4bd62d"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 11, "Öğrencilere karşı açık ve davetkar bir duruş mu sergiliyor?", null },
                    { new Guid("836b3644-9d48-42f6-b08f-cd1938ec0645"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 57, "Ders tekrar izlenmeli miydi, yoksa tek seferde anlaşılır mıydı?", null },
                    { new Guid("8b7cf1a6-a1f9-46fa-bd04-6655b20dea5a"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 43, "Görsel materyaller öğrenmeyi destekliyor mu, yoksa dikkat dağıtıyor mu?", null },
                    { new Guid("8d793010-a9a1-4697-8485-dbf677147708"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 42, "Tahtayı/ekranı düzenli ve okunabilir şekilde mi kullanıyor?", null },
                    { new Guid("90d513e8-9694-4148-acb1-d94594828e78"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 4, "Önemli kavramları vurgularken ses tonunu değiştiriyor mu?", null },
                    { new Guid("95d9ef26-014e-4cf8-bfba-ee9fd087c040"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 28, "Öğrencilerin kendi fikirlerini ifade etmelerine alan açıyor mu?", null },
                    { new Guid("9a145f43-890a-4ba4-8378-d9f5f650cb0b"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 10, "El, kol hareketleri anlatımı destekliyor mu?", null },
                    { new Guid("9db1a10d-5730-4211-a06a-2b2dbf4e34d4"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 55, "Tüm öğrencilere eşit mesafede mi davranıyor, favoriler var mı?", null },
                    { new Guid("9ff67be6-7baf-41f7-8a75-beedc04e2af7"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 16, "Konuyu günlük hayatla ilişkilendiriyor mu, somut örnekler veriyor mu?", null },
                    { new Guid("a3627758-cfb1-4467-a6f0-c0345d5e295a"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 37, "Disiplin müdahalesi varsa nasıl yapılıyor — saygılı mı, agresif mi?", null },
                    { new Guid("a8dd6e2d-fe9f-4d0a-a420-d2f6a74cd37c"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 46, "Öğrencilerin genel ilgi ve odaklanma düzeyi nasıl?", null },
                    { new Guid("ae8ad6ad-5196-4ab4-9344-4b6bc850ac6d"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 45, "Materyaller tüm öğrencilerin görebileceği şekilde konumlandırılmış mı?", null },
                    { new Guid("b031a62c-6fea-4515-8275-ef74d9f1787b"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 23, "Öğrencilere düşünme zamanı (bekleme süresi) veriyor mu?", null },
                    { new Guid("b048c402-5833-43d5-9f2c-9e804d300a2c"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 12, "Yüz ifadeleri anlatılanlarla uyumlu mu?", null },
                    { new Guid("b4bfdad0-09ff-4122-aa36-4df4333e4c54"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 32, "Cevabı bilmeyen öğrenciye nasıl yaklaşıyor?", null },
                    { new Guid("bb5d7216-1ce3-4849-839a-07997782348b"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 30, "Yanlış cevaplarda öğrenciyi küçük düşürüyor mu, yoksa yapıcı mı yaklaşıyor?", null },
                    { new Guid("bc6eca6f-edc8-4a9c-b1c1-f7b9e0b5b0af"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 3, "Öğretmen monoton mu konuşuyor, yoksa ses tonunda çeşitlilik var mı?", null },
                    { new Guid("c524dbe8-e652-4efc-bbda-174f67746268"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 50, "Öğretmen-öğrenci ilişkisi nasıl görünüyor — saygılı, mesafeli, sıcak?", null },
                    { new Guid("cc869755-0bc3-4193-b2cf-e9fb768e81b2"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 33, "Geri bildirimler spesifik mi (\"aferin\" gibi genel mi, yoksa \"şunu iyi yaptın çünkü…\" gibi açıklayıcı mı)?", null },
                    { new Guid("d1a6b3ea-e6f1-4e2c-a0a3-1e70a3f1f63b"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 17, "Karmaşık kavramları basitten karmaşığa doğru açıklıyor mu?", null },
                    { new Guid("d2a9a0ce-c61a-4617-833c-a253589dc3fb"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 54, "Öğrenme güçlüğü çeken öğrenciye özel ilgi gösteriyor mu?", null },
                    { new Guid("d5124406-5e77-43ba-94e5-fbff4e07552e"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 59, "Öğretmenin gelişim alanları nelerdir — somut önerilerle?", null },
                    { new Guid("d8da1678-c2a8-45ac-9b4a-d3072871f9ca"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 36, "Derse odaklanamayan ya da konuşan öğrencilerle nasıl başa çıkıyor?", null },
                    { new Guid("d94b00a5-174c-417c-a721-ac74b02f2d19"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 60, "Bu ders için genel bir başarı puanı verilseydi ne olurdu (1-10)?", null },
                    { new Guid("ddc37151-c9b2-4669-8eaf-6e1edf1be9a7"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 24, "Öğrenci cevaplarını dinliyor mu, yoksa cevabı kendisi mi tamamlıyor?", null },
                    { new Guid("ec0099df-5200-415f-9d22-f79fe2382985"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 20, "Ders hedefine ulaşıldı mı — dersin sonunda öğrenciler konuyu anlamış görünüyor mu?", null },
                    { new Guid("ec9c63b4-774c-44b7-a0dd-8b1cc3efe3e8"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 27, "Grup çalışması ya da işbirlikli öğrenme var mı?", null },
                    { new Guid("effec866-edc6-491c-bc1f-aa7e9f248b1b"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 26, "Gönüllü olmayan öğrencileri derse nasıl dahil ediyor?", null },
                    { new Guid("f28aae8b-d0d5-4d60-8588-f05aa382dd89"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 40, "Beklenmedik durumlara (teknik sorun, öğrenci sorusu, vs.) nasıl tepki veriyor?", null },
                    { new Guid("f41ca638-9007-4844-a009-dd1be26ddc64"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 21, "Öğrencilere ne sıklıkla soru soruyor?", null },
                    { new Guid("fc1af23b-d290-49cc-ba16-55e2717b8c33"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 1, "Öğretmenin sesi net ve anlaşılır mı, yoksa çok alçak/yüksek mi konuşuyor?", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EvaluationQuestions_CriteriaId_Order",
                table: "EvaluationQuestions",
                columns: new[] { "CriteriaId", "Order" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvaluationQuestions");

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "EvaluationCriteria",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$QoH.mUTjWK7BWNMhgTtKSufEUW6ikS/0fYXwcnsNRvHEyV9uR1M2W");
        }
    }
}
