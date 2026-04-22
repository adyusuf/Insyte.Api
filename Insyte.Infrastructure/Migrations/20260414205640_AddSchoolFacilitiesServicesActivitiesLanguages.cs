using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolFacilitiesServicesActivitiesLanguages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolActivities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Activity = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolActivities_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolFacilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Facility = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolFacilities_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolLanguages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolLanguages_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Service = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolServices_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$QoH.mUTjWK7BWNMhgTtKSufEUW6ikS/0fYXwcnsNRvHEyV9uR1M2W");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolActivities_SchoolId_Activity",
                table: "SchoolActivities",
                columns: new[] { "SchoolId", "Activity" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFacilities_SchoolId_Facility",
                table: "SchoolFacilities",
                columns: new[] { "SchoolId", "Facility" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolLanguages_SchoolId_Language",
                table: "SchoolLanguages",
                columns: new[] { "SchoolId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolServices_SchoolId_Service",
                table: "SchoolServices",
                columns: new[] { "SchoolId", "Service" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchoolActivities");

            migrationBuilder.DropTable(
                name: "SchoolFacilities");

            migrationBuilder.DropTable(
                name: "SchoolLanguages");

            migrationBuilder.DropTable(
                name: "SchoolServices");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$oZCXAowxR3a5lv6ZwM0xsuF7K/eN8VlfewNLir7R/xLiE32sjST06");
        }
    }
}
