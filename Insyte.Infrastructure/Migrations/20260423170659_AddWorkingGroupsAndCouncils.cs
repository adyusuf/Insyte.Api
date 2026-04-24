using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingGroupsAndCouncils : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("01098f9e-566d-42c4-9ad7-c43c30c1d253"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("014e083a-9c33-4cd6-82cc-3137fd185846"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("023c1dcf-a407-4a12-a665-093366a7df07"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("05ce08dd-3203-4e95-97fc-a9da1fb0dc9d"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("07a4b017-7e8c-4fe4-b418-a41207593b67"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("08381de1-ed3f-4d45-acfa-ae57fea26105"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("0f42af9b-cff7-4d7b-8b71-c2ad4811dabd"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("114c018c-3c53-4ec5-82be-29843d01de63"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("17396e2c-000a-4cc8-926f-a825477a5ab7"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("1955ef00-24b4-4052-977a-06354d397080"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2055e96c-4865-45d1-b5bc-77a5159bcb52"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("210ac981-4f3a-4080-9039-b51887a415ee"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("223ef81e-f374-475f-89ac-83d04cca2ad2"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2709dd27-84b9-4163-952e-b6f8664097b4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2718cb4a-84ad-4103-a324-562d3f934df4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2fe233bf-1201-4883-982a-e50723136acf"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("35de297e-465d-4213-9ca7-56d7e14f2e7d"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("3b7b2de9-a9ca-4028-b63e-e53f6ac3b974"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("3c1d1e9a-4a66-4bc2-9487-39b4e63d4c72"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("3eabb661-a029-468f-af1e-51552f4422e3"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("42bd976c-8abf-42ab-b901-e15d8d839036"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("4ebcdb3b-9707-4b9a-9426-2c4ac8307f3b"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("4edf6f71-c5c7-4c85-b2f1-44f0e7e13064"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("500c595d-033a-4519-a5a0-757fc6a715ca"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("5f28d1a7-9fb6-4f0d-bc61-98990fd66c97"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("626955c6-6eb1-42c3-905c-77cff0c66f4d"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("654df319-08b2-43bb-a4a8-9b74bf55748c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("6a9d92db-0897-4058-b656-85d54fc99786"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("6ebde72c-1d11-4121-8ff1-da8f6e8a157e"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("702aa571-bf16-44c8-bdd7-1340611a5781"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("70d4c1de-a375-477d-9fc8-93b383a22130"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("71d68ea9-5d32-444c-a600-b9a36985a3b5"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("76635c4d-a48d-42f0-9f31-3ebdcf2710d3"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("76b7378f-a4e6-4aa2-90f5-b46cdd9e5b50"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("79d0e1bd-6a64-4ff0-aff4-d9f9bab68416"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("7de43be5-fa86-4566-9acf-5a8d3f3b7038"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("7e1060d6-afa6-49a9-854f-e7cab2aa6e17"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("8c9ce5cc-c5fe-4d8f-9b93-37c96f34ef4b"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("9039a6bc-c359-457c-8bf7-30c10c439aec"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("9cf4e3c0-c5f8-4c8a-a42c-d22b02662e70"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("9d9fc366-ed57-4e85-a563-7aa4d8e3dc11"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("a2caf354-4ba6-42c9-912b-5b3567e99244"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("a2d88610-d48c-433e-8624-2794730ce4a4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("acc3fbc4-9876-43d6-8e95-ac163b06e8b0"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ad138edb-3a0f-4bc4-836f-82817aa41b4e"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ad51468b-4e61-4e48-8708-70aad58f10a6"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ada828cb-33d1-418b-be1b-abe3193daed0"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("bc3efa96-3b8b-45a6-8f84-1d0e0c2e49a4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("c02603d4-5620-40dc-97a1-9861d9dd818e"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("c1860b06-5f17-4e4a-9d2a-c526be59321d"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("c305bd6e-b1d8-484d-a13d-e54b37857bbe"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("cb706098-8eed-43d8-a3a4-d1436072cfb9"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("cdc2d783-0720-4d46-843e-8c70bbda07a6"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("d4071b27-401d-4673-8188-8a9ec015adf7"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("d5e1f0ac-8b51-4853-81e8-3f8d394a4559"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("f472b428-591f-4d18-8610-b1a1b2ba8806"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("f678e27b-7032-4730-8617-1345698011e2"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("fa59e0b4-46c8-49ef-a5f1-59b0e8b97477"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("fdcabcbf-5099-4e8f-94f5-3f12beef9293"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("fdd43b83-451b-41a2-84bc-07384f7a02e3"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("fe1c9003-7923-4770-add8-3966bb61f942"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("0eb7bb23-9b37-473b-b219-b864ff6519f1"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("2522d2b6-efce-4ebf-85ff-058abace2099"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("6e132f98-c039-4b4a-a810-c0388fb2c4d8"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("732f49c4-9337-469e-8189-ae84a8a56e68"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("8ff5e5f2-8b16-4fcd-a09c-a2e88621b935"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("9c660cb8-be82-4a60-a8d6-e6458e9853b4"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("a2b33f05-ccc1-4559-938a-451d03091b3b"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e059ab8c-d18a-4180-ad38-fcd2a49e3e58"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("e59da1f4-acf1-47fb-b49e-a94faf803c07"));

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: new Guid("fe331669-b544-4308-af83-6e30a7ec78ac"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000003"));

            migrationBuilder.CreateTable(
                name: "Councils",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Councils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Councils_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingGroups_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouncilMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouncilId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouncilMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouncilMembers_Councils_CouncilId",
                        column: x => x.CouncilId,
                        principalTable: "Councils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouncilMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingGroupMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkingGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: true),
                    AssignedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingGroupMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingGroupMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingGroupMembers_WorkingGroups_WorkingGroupId",
                        column: x => x.WorkingGroupId,
                        principalTable: "WorkingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EvaluationQuestions",
                columns: new[] { "Id", "Category", "CreatedAt", "CriteriaId", "IsActive", "Order", "Question", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01af916a-811f-4d0c-a6fe-4a73e323eab4"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 29, "Öğrenci cevapları doğru olduğunda nasıl geri bildirim veriyor?", null },
                    { new Guid("03552865-15bb-4db9-9378-7f74da68423e"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 3, "Öğretmen monoton mu konuşuyor, yoksa ses tonunda çeşitlilik var mı?", null },
                    { new Guid("058bbde2-3fd0-47fb-b05e-6464900b471f"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 1, "Öğretmenin sesi net ve anlaşılır mı, yoksa çok alçak/yüksek mi konuşuyor?", null },
                    { new Guid("0d7bd867-9b68-4629-86c6-9784282941d1"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 42, "Tahtayı/ekranı düzenli ve okunabilir şekilde mi kullanıyor?", null },
                    { new Guid("11a40eff-9df7-4d95-a4f1-c3f64e1d8551"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 45, "Materyaller tüm öğrencilerin görebileceği şekilde konumlandırılmış mı?", null },
                    { new Guid("16e7b4ef-cd0f-4d2f-94f1-23643e0b12ce"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 47, "Öğrenciler ders sırasında ne yapıyor — not mu alıyor, telefonla mı ilgileniyor, konuşuyor mu?", null },
                    { new Guid("17f87281-2043-42c7-add5-6c7ad637d6da"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 19, "Ders sonunda özet ya da tekrar yapıyor mu?", null },
                    { new Guid("1a35ff6e-be2b-48f2-af81-b119d1c44edf"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 43, "Görsel materyaller öğrenmeyi destekliyor mu, yoksa dikkat dağıtıyor mu?", null },
                    { new Guid("1bee0fab-b412-4618-a6b1-d7ca89057d41"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 50, "Öğretmen-öğrenci ilişkisi nasıl görünüyor — saygılı, mesafeli, sıcak?", null },
                    { new Guid("25d673b4-908a-4ed2-bee3-53c3a6a63a40"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 46, "Öğrencilerin genel ilgi ve odaklanma düzeyi nasıl?", null },
                    { new Guid("2628b454-442c-4bae-afb3-33fd2ca06ae4"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 22, "Soruları sadece belli öğrencilere mi, yoksa tüm sınıfa mı yöneltiyor?", null },
                    { new Guid("27f6027d-b3e0-4a9e-96c4-7253af85d6e2"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 14, "Konu anlatımı mantıksal bir sırayla ilerliyor mu?", null },
                    { new Guid("2963e095-5b1e-4847-a19c-0ea6a8f4d20b"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 2, "Konuşma hızı uygun mu — çok hızlı veya çok yavaş mı?", null },
                    { new Guid("2c9ba458-2a3e-41f4-9aec-4c4e172023c7"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 53, "Farklı öğrenme hızlarındaki öğrencilere uyum sağlıyor mu?", null },
                    { new Guid("2d0cce69-0687-4354-b89b-7711da049a60"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 4, "Önemli kavramları vurgularken ses tonunu değiştiriyor mu?", null },
                    { new Guid("32de9310-1ad3-47cf-adef-956a8c720de8"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 55, "Tüm öğrencilere eşit mesafede mi davranıyor, favoriler var mı?", null },
                    { new Guid("32f441db-6747-4803-b1dd-ba5f63158826"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 41, "Kullanılan materyaller (slayt, tahta, video, vs.) içerikle uyumlu ve kaliteli mi?", null },
                    { new Guid("32ffffe4-ef90-4f16-9228-a13bc5893a1c"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 15, "Öğrettiği bilgiler doğru mu — kavram veya bilgi hatası var mı?", null },
                    { new Guid("3c9f5eef-3ca0-4ee9-b862-cfc505984dd1"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 5, "Türkçeyi (ya da ders dilini) doğru ve akıcı kullanıyor mu?", null },
                    { new Guid("4514ceb1-2ec6-4577-a399-ccda4c433d23"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 52, "Dersin hangi anlarında öğrenci enerjisi yükseliyor ya da düşüyor?", null },
                    { new Guid("47368a71-c320-40c8-bf70-570c60f4b216"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 25, "Öğrencilerin birbirini dinlemesini ve tartışmasını teşvik ediyor mu?", null },
                    { new Guid("4d8a83c2-ba32-4b0a-b945-26db4cc7dd7a"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 16, "Konuyu günlük hayatla ilişkilendiriyor mu, somut örnekler veriyor mu?", null },
                    { new Guid("4f65f304-19ca-47a6-9579-100d3df19278"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 38, "Zamanı iyi yönetiyor mu — ders planladığı gibi ilerliyor mu?", null },
                    { new Guid("59b7aa4d-1886-4d54-a01a-b0a336905a6f"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 31, "Öğrencinin sorusunu doğru anlayıp yanıtlıyor mu?", null },
                    { new Guid("5b251256-d40c-422a-a102-b82bd12b942f"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 48, "Öğrencilerin soru sorma ya da katılım istekliliği var mı?", null },
                    { new Guid("602d5eec-bdc0-4424-81c6-0c7eb6bfce03"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 33, "Geri bildirimler spesifik mi (\"aferin\" gibi genel mi, yoksa \"şunu iyi yaptın çünkü…\" gibi açıklayıcı mı)?", null },
                    { new Guid("6bda2993-60e0-4dd4-9de7-256f6888659c"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 23, "Öğrencilere düşünme zamanı (bekleme süresi) veriyor mu?", null },
                    { new Guid("6f81f1cc-d418-4f81-9537-d589f6b51141"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 20, "Ders hedefine ulaşıldı mı — dersin sonunda öğrenciler konuyu anlamış görünüyor mu?", null },
                    { new Guid("76e4a93c-6511-4123-85c6-824e862000d7"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 7, "Öğrencilere hitap ederken ne kadar samimi ve sıcak bir ton kullanıyor?", null },
                    { new Guid("78cc007a-ac3b-4cac-8548-d68b3c129208"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 18, "Önceki derslerle bağlantı kuruyor mu?", null },
                    { new Guid("79dc0e5b-fa1e-4be0-b414-15418a90692e"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 8, "Öğretmen sınıfı aktif olarak dolaşıyor mu, yoksa tek noktada mı duruyor?", null },
                    { new Guid("7e20f63a-2e79-4a10-9f77-7d935c865037"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 10, "El, kol hareketleri anlatımı destekliyor mu?", null },
                    { new Guid("85dbc5cb-5e78-45f6-bcc1-2b978f35dd00"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 9, "Göz teması kuruyor mu — sadece tahtaya mı bakıyor, öğrencilere mi?", null },
                    { new Guid("87901df8-f1f5-49c5-9bca-c612d4954441"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 30, "Yanlış cevaplarda öğrenciyi küçük düşürüyor mu, yoksa yapıcı mı yaklaşıyor?", null },
                    { new Guid("8a8344ce-2554-4e72-b58f-56c4605dc03f"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 36, "Derse odaklanamayan ya da konuşan öğrencilerle nasıl başa çıkıyor?", null },
                    { new Guid("92cf3727-c4c1-46b6-93de-c6ef48b0910f"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 57, "Ders tekrar izlenmeli miydi, yoksa tek seferde anlaşılır mıydı?", null },
                    { new Guid("96bd06ae-9b62-40bf-b32a-dc02a7e7c3f3"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 13, "Dersin başında öğrencilere neyi öğreneceklerini açıklıyor mu?", null },
                    { new Guid("96f9db6e-9ce1-44db-be34-783db04030e9"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 35, "Sınıfta genel düzen ve sessizlik sağlanabiliyor mu?", null },
                    { new Guid("9983723c-2f9e-49dc-9e05-49065822ad4d"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 44, "Teknolojiyi etkin mi kullanıyor?", null },
                    { new Guid("9ce77b91-0f9a-41a0-9709-0d6fdaa43a42"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 6, "Gereksiz dolgu sözler (yani, şey, hmm) ne sıklıkla kullanılıyor?", null },
                    { new Guid("9def99f6-992d-4c43-9db9-3640b3a779e5"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 26, "Gönüllü olmayan öğrencileri derse nasıl dahil ediyor?", null },
                    { new Guid("a019898b-0f58-445e-bf25-132e61d8a010"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 11, "Öğrencilere karşı açık ve davetkar bir duruş mu sergiliyor?", null },
                    { new Guid("a81f4e2c-3969-4b81-bcdb-a53d5a152d81"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 54, "Öğrenme güçlüğü çeken öğrenciye özel ilgi gösteriyor mu?", null },
                    { new Guid("ac26de94-f436-4d35-b664-0e2e1e2dd28c"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 58, "Öğretmenin en güçlü 3 yönü nedir?", null },
                    { new Guid("af01fe96-aef4-4cd6-8fb8-2cd9ebc4176c"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 61, "Bir sonraki derste öncelikli olarak iyileştirilmesi gereken tek şey ne?", null },
                    { new Guid("b5e1e614-8859-41f0-be04-ddebd99c7788"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 34, "Öğrencilerin anladığını nasıl kontrol ediyor — anlama soruları soruyor mu?", null },
                    { new Guid("b97015c8-fbce-42e9-8418-2f8f418b8f0c"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 37, "Disiplin müdahalesi varsa nasıl yapılıyor — saygılı mı, agresif mi?", null },
                    { new Guid("bc54be87-efaa-44f9-91f6-778d046da8e4"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 21, "Öğrencilere ne sıklıkla soru soruyor?", null },
                    { new Guid("c9bb7337-e80a-4632-9d39-163118daf3a5"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 56, "Bu dersi izleyen biri ne öğrendiğini açıklayabilir mi?", null },
                    { new Guid("ca8ec27f-74f6-4a6d-93c1-0278e1172217"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 28, "Öğrencilerin kendi fikirlerini ifade etmelerine alan açıyor mu?", null },
                    { new Guid("cc029d08-218a-4657-a14c-7a946fb4545c"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 51, "Öğrencilerin yüz ifadeleri ve beden dili neler söylüyor — sıkılmış mı, meraklı mı?", null },
                    { new Guid("db03de7c-da24-44f4-9fef-729784dfd348"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 32, "Cevabı bilmeyen öğrenciye nasıl yaklaşıyor?", null },
                    { new Guid("de9281f6-4236-4945-8668-200e018a5c22"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 39, "Geçişler (yeni konuya geçiş, aktivite değişimi) pürüzsüz mü?", null },
                    { new Guid("e0c6f777-aea8-4304-b903-38317e4a31ee"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 17, "Karmaşık kavramları basitten karmaşığa doğru açıklıyor mu?", null },
                    { new Guid("e57f0c79-538c-4f4a-9080-2985e96dcbd5"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 49, "Sınıfta pozitif, güvenli bir öğrenme ortamı var mı?", null },
                    { new Guid("ea58925d-e473-4f98-9b66-6033f59d0836"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 59, "Öğretmenin gelişim alanları nelerdir — somut önerilerle?", null },
                    { new Guid("ed1bb51e-be0c-43b1-a638-9bb6fa70989c"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 60, "Bu ders için genel bir başarı puanı verilseydi ne olurdu (1-10)?", null },
                    { new Guid("f6f8efe1-a6ea-4f5f-9077-60e082150bd6"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 27, "Grup çalışması ya da işbirlikli öğrenme var mı?", null },
                    { new Guid("f7b6fe72-3fba-407b-8f82-13f79f57f033"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 24, "Öğrenci cevaplarını dinliyor mu, yoksa cevabı kendisi mi tamamlıyor?", null },
                    { new Guid("f98de439-ca0d-4686-9d95-e6ed3c4fab00"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 12, "Yüz ifadeleri anlatılanlarla uyumlu mu?", null },
                    { new Guid("fe897cfb-0d44-4a8b-a3ea-13d726639fd7"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 40, "Beklenmedik durumlara (teknik sorun, öğrenci sorusu, vs.) nasıl tepki veriyor?", null }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$0BISi69986QCN8DQdYtLJ.q5KedTBUjt3rBNp2R8m26dsddJWhXii");

            migrationBuilder.CreateIndex(
                name: "IX_CouncilMembers_CouncilId_UserId",
                table: "CouncilMembers",
                columns: new[] { "CouncilId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouncilMembers_UserId",
                table: "CouncilMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Councils_SchoolId_Name",
                table: "Councils",
                columns: new[] { "SchoolId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroupMembers_UserId",
                table: "WorkingGroupMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroupMembers_WorkingGroupId_UserId",
                table: "WorkingGroupMembers",
                columns: new[] { "WorkingGroupId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroups_SchoolId_Name",
                table: "WorkingGroups",
                columns: new[] { "SchoolId", "Name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouncilMembers");

            migrationBuilder.DropTable(
                name: "WorkingGroupMembers");

            migrationBuilder.DropTable(
                name: "Councils");

            migrationBuilder.DropTable(
                name: "WorkingGroups");

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("01af916a-811f-4d0c-a6fe-4a73e323eab4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("03552865-15bb-4db9-9378-7f74da68423e"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("058bbde2-3fd0-47fb-b05e-6464900b471f"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("0d7bd867-9b68-4629-86c6-9784282941d1"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("11a40eff-9df7-4d95-a4f1-c3f64e1d8551"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("16e7b4ef-cd0f-4d2f-94f1-23643e0b12ce"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("17f87281-2043-42c7-add5-6c7ad637d6da"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("1a35ff6e-be2b-48f2-af81-b119d1c44edf"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("1bee0fab-b412-4618-a6b1-d7ca89057d41"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("25d673b4-908a-4ed2-bee3-53c3a6a63a40"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2628b454-442c-4bae-afb3-33fd2ca06ae4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("27f6027d-b3e0-4a9e-96c4-7253af85d6e2"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2963e095-5b1e-4847-a19c-0ea6a8f4d20b"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2c9ba458-2a3e-41f4-9aec-4c4e172023c7"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("2d0cce69-0687-4354-b89b-7711da049a60"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("32de9310-1ad3-47cf-adef-956a8c720de8"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("32f441db-6747-4803-b1dd-ba5f63158826"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("32ffffe4-ef90-4f16-9228-a13bc5893a1c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("3c9f5eef-3ca0-4ee9-b862-cfc505984dd1"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("4514ceb1-2ec6-4577-a399-ccda4c433d23"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("47368a71-c320-40c8-bf70-570c60f4b216"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("4d8a83c2-ba32-4b0a-b945-26db4cc7dd7a"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("4f65f304-19ca-47a6-9579-100d3df19278"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("59b7aa4d-1886-4d54-a01a-b0a336905a6f"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("5b251256-d40c-422a-a102-b82bd12b942f"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("602d5eec-bdc0-4424-81c6-0c7eb6bfce03"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("6bda2993-60e0-4dd4-9de7-256f6888659c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("6f81f1cc-d418-4f81-9537-d589f6b51141"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("76e4a93c-6511-4123-85c6-824e862000d7"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("78cc007a-ac3b-4cac-8548-d68b3c129208"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("79dc0e5b-fa1e-4be0-b414-15418a90692e"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("7e20f63a-2e79-4a10-9f77-7d935c865037"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("85dbc5cb-5e78-45f6-bcc1-2b978f35dd00"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("87901df8-f1f5-49c5-9bca-c612d4954441"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("8a8344ce-2554-4e72-b58f-56c4605dc03f"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("92cf3727-c4c1-46b6-93de-c6ef48b0910f"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("96bd06ae-9b62-40bf-b32a-dc02a7e7c3f3"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("96f9db6e-9ce1-44db-be34-783db04030e9"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("9983723c-2f9e-49dc-9e05-49065822ad4d"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("9ce77b91-0f9a-41a0-9709-0d6fdaa43a42"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("9def99f6-992d-4c43-9db9-3640b3a779e5"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("a019898b-0f58-445e-bf25-132e61d8a010"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("a81f4e2c-3969-4b81-bcdb-a53d5a152d81"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ac26de94-f436-4d35-b664-0e2e1e2dd28c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("af01fe96-aef4-4cd6-8fb8-2cd9ebc4176c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("b5e1e614-8859-41f0-be04-ddebd99c7788"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("b97015c8-fbce-42e9-8418-2f8f418b8f0c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("bc54be87-efaa-44f9-91f6-778d046da8e4"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("c9bb7337-e80a-4632-9d39-163118daf3a5"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ca8ec27f-74f6-4a6d-93c1-0278e1172217"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("cc029d08-218a-4657-a14c-7a946fb4545c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("db03de7c-da24-44f4-9fef-729784dfd348"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("de9281f6-4236-4945-8668-200e018a5c22"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("e0c6f777-aea8-4304-b903-38317e4a31ee"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("e57f0c79-538c-4f4a-9080-2985e96dcbd5"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ea58925d-e473-4f98-9b66-6033f59d0836"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("ed1bb51e-be0c-43b1-a638-9bb6fa70989c"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("f6f8efe1-a6ea-4f5f-9077-60e082150bd6"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("f7b6fe72-3fba-407b-8f82-13f79f57f033"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("f98de439-ca0d-4686-9d95-e6ed3c4fab00"));

            migrationBuilder.DeleteData(
                table: "EvaluationQuestions",
                keyColumn: "Id",
                keyValue: new Guid("fe897cfb-0d44-4a8b-a3ea-13d726639fd7"));

            migrationBuilder.InsertData(
                table: "EvaluationQuestions",
                columns: new[] { "Id", "Category", "CreatedAt", "CriteriaId", "IsActive", "Order", "Question", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01098f9e-566d-42c4-9ad7-c43c30c1d253"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 57, "Ders tekrar izlenmeli miydi, yoksa tek seferde anlaşılır mıydı?", null },
                    { new Guid("014e083a-9c33-4cd6-82cc-3137fd185846"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 54, "Öğrenme güçlüğü çeken öğrenciye özel ilgi gösteriyor mu?", null },
                    { new Guid("023c1dcf-a407-4a12-a665-093366a7df07"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 60, "Bu ders için genel bir başarı puanı verilseydi ne olurdu (1-10)?", null },
                    { new Guid("05ce08dd-3203-4e95-97fc-a9da1fb0dc9d"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 34, "Öğrencilerin anladığını nasıl kontrol ediyor — anlama soruları soruyor mu?", null },
                    { new Guid("07a4b017-7e8c-4fe4-b418-a41207593b67"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 30, "Yanlış cevaplarda öğrenciyi küçük düşürüyor mu, yoksa yapıcı mı yaklaşıyor?", null },
                    { new Guid("08381de1-ed3f-4d45-acfa-ae57fea26105"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 58, "Öğretmenin en güçlü 3 yönü nedir?", null },
                    { new Guid("0f42af9b-cff7-4d7b-8b71-c2ad4811dabd"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 5, "Türkçeyi (ya da ders dilini) doğru ve akıcı kullanıyor mu?", null },
                    { new Guid("114c018c-3c53-4ec5-82be-29843d01de63"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 39, "Geçişler (yeni konuya geçiş, aktivite değişimi) pürüzsüz mü?", null },
                    { new Guid("17396e2c-000a-4cc8-926f-a825477a5ab7"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 36, "Derse odaklanamayan ya da konuşan öğrencilerle nasıl başa çıkıyor?", null },
                    { new Guid("1955ef00-24b4-4052-977a-06354d397080"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 26, "Gönüllü olmayan öğrencileri derse nasıl dahil ediyor?", null },
                    { new Guid("2055e96c-4865-45d1-b5bc-77a5159bcb52"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 55, "Tüm öğrencilere eşit mesafede mi davranıyor, favoriler var mı?", null },
                    { new Guid("210ac981-4f3a-4080-9039-b51887a415ee"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 23, "Öğrencilere düşünme zamanı (bekleme süresi) veriyor mu?", null },
                    { new Guid("223ef81e-f374-475f-89ac-83d04cca2ad2"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 52, "Dersin hangi anlarında öğrenci enerjisi yükseliyor ya da düşüyor?", null },
                    { new Guid("2709dd27-84b9-4163-952e-b6f8664097b4"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 13, "Dersin başında öğrencilere neyi öğreneceklerini açıklıyor mu?", null },
                    { new Guid("2718cb4a-84ad-4103-a324-562d3f934df4"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 51, "Öğrencilerin yüz ifadeleri ve beden dili neler söylüyor — sıkılmış mı, meraklı mı?", null },
                    { new Guid("2fe233bf-1201-4883-982a-e50723136acf"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 32, "Cevabı bilmeyen öğrenciye nasıl yaklaşıyor?", null },
                    { new Guid("35de297e-465d-4213-9ca7-56d7e14f2e7d"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 19, "Ders sonunda özet ya da tekrar yapıyor mu?", null },
                    { new Guid("3b7b2de9-a9ca-4028-b63e-e53f6ac3b974"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 47, "Öğrenciler ders sırasında ne yapıyor — not mu alıyor, telefonla mı ilgileniyor, konuşuyor mu?", null },
                    { new Guid("3c1d1e9a-4a66-4bc2-9487-39b4e63d4c72"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 61, "Bir sonraki derste öncelikli olarak iyileştirilmesi gereken tek şey ne?", null },
                    { new Guid("3eabb661-a029-468f-af1e-51552f4422e3"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 40, "Beklenmedik durumlara (teknik sorun, öğrenci sorusu, vs.) nasıl tepki veriyor?", null },
                    { new Guid("42bd976c-8abf-42ab-b901-e15d8d839036"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 41, "Kullanılan materyaller (slayt, tahta, video, vs.) içerikle uyumlu ve kaliteli mi?", null },
                    { new Guid("4ebcdb3b-9707-4b9a-9426-2c4ac8307f3b"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 49, "Sınıfta pozitif, güvenli bir öğrenme ortamı var mı?", null },
                    { new Guid("4edf6f71-c5c7-4c85-b2f1-44f0e7e13064"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 27, "Grup çalışması ya da işbirlikli öğrenme var mı?", null },
                    { new Guid("500c595d-033a-4519-a5a0-757fc6a715ca"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 1, "Öğretmenin sesi net ve anlaşılır mı, yoksa çok alçak/yüksek mi konuşuyor?", null },
                    { new Guid("5f28d1a7-9fb6-4f0d-bc61-98990fd66c97"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 46, "Öğrencilerin genel ilgi ve odaklanma düzeyi nasıl?", null },
                    { new Guid("626955c6-6eb1-42c3-905c-77cff0c66f4d"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 59, "Öğretmenin gelişim alanları nelerdir — somut önerilerle?", null },
                    { new Guid("654df319-08b2-43bb-a4a8-9b74bf55748c"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 17, "Karmaşık kavramları basitten karmaşığa doğru açıklıyor mu?", null },
                    { new Guid("6a9d92db-0897-4058-b656-85d54fc99786"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 29, "Öğrenci cevapları doğru olduğunda nasıl geri bildirim veriyor?", null },
                    { new Guid("6ebde72c-1d11-4121-8ff1-da8f6e8a157e"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 22, "Soruları sadece belli öğrencilere mi, yoksa tüm sınıfa mı yöneltiyor?", null },
                    { new Guid("702aa571-bf16-44c8-bdd7-1340611a5781"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 8, "Öğretmen sınıfı aktif olarak dolaşıyor mu, yoksa tek noktada mı duruyor?", null },
                    { new Guid("70d4c1de-a375-477d-9fc8-93b383a22130"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 15, "Öğrettiği bilgiler doğru mu — kavram veya bilgi hatası var mı?", null },
                    { new Guid("71d68ea9-5d32-444c-a600-b9a36985a3b5"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 42, "Tahtayı/ekranı düzenli ve okunabilir şekilde mi kullanıyor?", null },
                    { new Guid("76635c4d-a48d-42f0-9f31-3ebdcf2710d3"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 48, "Öğrencilerin soru sorma ya da katılım istekliliği var mı?", null },
                    { new Guid("76b7378f-a4e6-4aa2-90f5-b46cdd9e5b50"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 37, "Disiplin müdahalesi varsa nasıl yapılıyor — saygılı mı, agresif mi?", null },
                    { new Guid("79d0e1bd-6a64-4ff0-aff4-d9f9bab68416"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 45, "Materyaller tüm öğrencilerin görebileceği şekilde konumlandırılmış mı?", null },
                    { new Guid("7de43be5-fa86-4566-9acf-5a8d3f3b7038"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 10, "El, kol hareketleri anlatımı destekliyor mu?", null },
                    { new Guid("7e1060d6-afa6-49a9-854f-e7cab2aa6e17"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 16, "Konuyu günlük hayatla ilişkilendiriyor mu, somut örnekler veriyor mu?", null },
                    { new Guid("8c9ce5cc-c5fe-4d8f-9b93-37c96f34ef4b"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 44, "Teknolojiyi etkin mi kullanıyor?", null },
                    { new Guid("9039a6bc-c359-457c-8bf7-30c10c439aec"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 21, "Öğrencilere ne sıklıkla soru soruyor?", null },
                    { new Guid("9cf4e3c0-c5f8-4c8a-a42c-d22b02662e70"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 33, "Geri bildirimler spesifik mi (\"aferin\" gibi genel mi, yoksa \"şunu iyi yaptın çünkü…\" gibi açıklayıcı mı)?", null },
                    { new Guid("9d9fc366-ed57-4e85-a563-7aa4d8e3dc11"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 7, "Öğrencilere hitap ederken ne kadar samimi ve sıcak bir ton kullanıyor?", null },
                    { new Guid("a2caf354-4ba6-42c9-912b-5b3567e99244"), "💬 Geri Bildirim & Soru-Cevap", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000005"), true, 31, "Öğrencinin sorusunu doğru anlayıp yanıtlıyor mu?", null },
                    { new Guid("a2d88610-d48c-433e-8624-2794730ce4a4"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 3, "Öğretmen monoton mu konuşuyor, yoksa ses tonunda çeşitlilik var mı?", null },
                    { new Guid("acc3fbc4-9876-43d6-8e95-ac163b06e8b0"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 9, "Göz teması kuruyor mu — sadece tahtaya mı bakıyor, öğrencilere mi?", null },
                    { new Guid("ad138edb-3a0f-4bc4-836f-82817aa41b4e"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 38, "Zamanı iyi yönetiyor mu — ders planladığı gibi ilerliyor mu?", null },
                    { new Guid("ad51468b-4e61-4e48-8708-70aad58f10a6"), "🏫 Sınıf Yönetimi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000004"), true, 35, "Sınıfta genel düzen ve sessizlik sağlanabiliyor mu?", null },
                    { new Guid("ada828cb-33d1-418b-be1b-abe3193daed0"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 28, "Öğrencilerin kendi fikirlerini ifade etmelerine alan açıyor mu?", null },
                    { new Guid("bc3efa96-3b8b-45a6-8f84-1d0e0c2e49a4"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 11, "Öğrencilere karşı açık ve davetkar bir duruş mu sergiliyor?", null },
                    { new Guid("c02603d4-5620-40dc-97a1-9861d9dd818e"), "🧍 Beden Dili & Fiziksel Varlık", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 12, "Yüz ifadeleri anlatılanlarla uyumlu mu?", null },
                    { new Guid("c1860b06-5f17-4e4a-9d2a-c526be59321d"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 24, "Öğrenci cevaplarını dinliyor mu, yoksa cevabı kendisi mi tamamlıyor?", null },
                    { new Guid("c305bd6e-b1d8-484d-a13d-e54b37857bbe"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 20, "Ders hedefine ulaşıldı mı — dersin sonunda öğrenciler konuyu anlamış görünüyor mu?", null },
                    { new Guid("cb706098-8eed-43d8-a3a4-d1436072cfb9"), "🖥️ Materyal & Araç Kullanımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000006"), true, 43, "Görsel materyaller öğrenmeyi destekliyor mu, yoksa dikkat dağıtıyor mu?", null },
                    { new Guid("cdc2d783-0720-4d46-843e-8c70bbda07a6"), "🙋 Öğrenci Katılımı", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000003"), true, 25, "Öğrencilerin birbirini dinlemesini ve tartışmasını teşvik ediyor mu?", null },
                    { new Guid("d4071b27-401d-4673-8188-8a9ec015adf7"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 14, "Konu anlatımı mantıksal bir sırayla ilerliyor mu?", null },
                    { new Guid("d5e1f0ac-8b51-4853-81e8-3f8d394a4559"), "👥 Öğrenci Davranışı & Sınıf Atmosferi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000008"), true, 50, "Öğretmen-öğrenci ilişkisi nasıl görünüyor — saygılı, mesafeli, sıcak?", null },
                    { new Guid("f472b428-591f-4d18-8610-b1a1b2ba8806"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 2, "Konuşma hızı uygun mu — çok hızlı veya çok yavaş mı?", null },
                    { new Guid("f678e27b-7032-4730-8617-1345698011e2"), "🔄 Genel Ders Değerlendirmesi", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000010"), true, 56, "Bu dersi izleyen biri ne öğrendiğini açıklayabilir mi?", null },
                    { new Guid("fa59e0b4-46c8-49ef-a5f1-59b0e8b97477"), "📚 Pedagoji & Ders İçeriği", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000001"), true, 18, "Önceki derslerle bağlantı kuruyor mu?", null },
                    { new Guid("fdcabcbf-5099-4e8f-94f5-3f12beef9293"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 4, "Önemli kavramları vurgularken ses tonunu değiştiriyor mu?", null },
                    { new Guid("fdd43b83-451b-41a2-84bc-07384f7a02e3"), "🗣️ İletişim & Ses", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000002"), true, 6, "Gereksiz dolgu sözler (yani, şey, hmm) ne sıklıkla kullanılıyor?", null },
                    { new Guid("fe1c9003-7923-4770-add8-3966bb61f942"), "🌍 Kapsayıcılık & Farklılaştırma", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("10000000-0000-0000-0000-000000000007"), true, 53, "Farklı öğrenme hızlarındaki öğrencilere uyum sağlıyor mu?", null }
                });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "City", "CreatedAt", "EducationSystem", "Email", "InstitutionType", "IsActive", "Latitude", "LiseType", "LogoPath", "Longitude", "Name", "Phone", "SchoolType", "UpdatedAt", "Website" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "Mimar Sinan Cad. No:45", "İstanbul", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "info@ataturk.edu.tr", "Devlet", true, null, null, null, null, "Atatürk Ortaokulu", "0212 555 1234", "Ortaokul", null, "www.ataturk.edu.tr" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "Nispetiye Cad. No:123", "İstanbul", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "MEBOdakliEgitim", "info@dostlisesi.edu.tr", "Ozel", true, null, "AnadoluLisesi", null, null, "Özel Dost Lisesi", "0212 555 5678", "Lise", null, "www.dostlisesi.edu.tr" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "Ulus Mah. Atatürk Bulv. No:78", "Ankara", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "info@biltek.edu.tr", "Devlet", true, null, null, null, null, "Bilim ve Teknoloji İlkokulu", "0312 555 9012", "Ilkokul", null, "www.biltek.edu.tr" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$5a8W62bzMqef/Qrg.e30k.z.MhNfs3zx5nKq89nYzyZd6ssXzKOi.");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "ayse.yilmaz@school.edu.tr", "Ayşe", true, "Yılmaz", "$2a$11$GKkJX06RZWApsb6H4UESBeNdRYjYLdKFnTa1zlJ0aVwQyPqzQ8IWu", "Teacher", null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "mehmet.kaya@school.edu.tr", "Mehmet", true, "Kaya", "$2a$11$mDjpMxxAir825XCbGrRJe.o2K8P3pZUpymTlMfllQHvo2WhLD4k1S", "Teacher", null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "fatih.demir@school.edu.tr", "Fatih", true, "Demir", "$2a$11$R1XYbZuOrwbZFEWEkkAD1.P3tHnqUKC/.H7H9D4UkRqqxOuSWbp66", "Teacher", null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "sule.can@school.edu.tr", "Şule", true, "Can", "$2a$11$boxOlkDhvni81V9j2LTZx.e4bHOAmFsrqeE1lD5V2ujgMfE4gMUIa", "Teacher", null },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "ali.ozturk@school.edu.tr", "Ali", true, "Öztürk", "$2a$11$ZN6lgGS/s/2Ho839TU.Ye.0pF22krR9.12NMVQjITHk4/mTjZEwqK", "Teacher", null }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Branch", "CreatedAt", "Description", "IsActive", "Level", "Name", "SchoolId", "UpdatedAt", "WeeklyHours" },
                values: new object[,]
                {
                    { new Guid("0eb7bb23-9b37-473b-b219-b864ff6519f1"), "İngilizce", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "İngilizce dil öğretimi", true, "6-7", "İngilizce", new Guid("20000000-0000-0000-0000-000000000001"), null, 3 },
                    { new Guid("2522d2b6-efce-4ebf-85ff-058abace2099"), "Matematik", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Matematik öğretimi", true, "3", "Matematik", new Guid("20000000-0000-0000-0000-000000000003"), null, 5 },
                    { new Guid("6e132f98-c039-4b4a-a810-c0388fb2c4d8"), "Matematik", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Matematik öğretimi", true, "6-7", "Matematik", new Guid("20000000-0000-0000-0000-000000000001"), null, 4 },
                    { new Guid("732f49c4-9337-469e-8189-ae84a8a56e68"), "Fen", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Fen ve teknoloji öğretimi", true, "6-7", "Fen Bilgisi", new Guid("20000000-0000-0000-0000-000000000001"), null, 3 },
                    { new Guid("8ff5e5f2-8b16-4fcd-a09c-a2e88621b935"), "Türkçe", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Türkçe dilini ve edebiyatını öğretme", true, "3", "Türkçe", new Guid("20000000-0000-0000-0000-000000000003"), null, 5 },
                    { new Guid("9c660cb8-be82-4a60-a8d6-e6458e9853b4"), "Fen", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Fizik öğretimi", true, "10", "Fizik", new Guid("20000000-0000-0000-0000-000000000002"), null, 3 },
                    { new Guid("a2b33f05-ccc1-4559-938a-451d03091b3b"), "Türkçe", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Türkçe dilini ve edebiyatını öğretme", true, "10", "Türkçe", new Guid("20000000-0000-0000-0000-000000000002"), null, 3 },
                    { new Guid("e059ab8c-d18a-4180-ad38-fcd2a49e3e58"), "İngilizce", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "İngilizce dil öğretimi", true, "10", "İngilizce", new Guid("20000000-0000-0000-0000-000000000002"), null, 3 },
                    { new Guid("e59da1f4-acf1-47fb-b49e-a94faf803c07"), "Matematik", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Matematik öğretimi", true, "10", "Matematik", new Guid("20000000-0000-0000-0000-000000000002"), null, 4 },
                    { new Guid("fe331669-b544-4308-af83-6e30a7ec78ac"), "Türkçe", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Türkçe dilini ve edebiyatını öğretme", true, "6-7", "Türkçe", new Guid("20000000-0000-0000-0000-000000000001"), null, 4 }
                });
        }
    }
}
