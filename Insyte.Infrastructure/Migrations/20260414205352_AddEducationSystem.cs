using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEducationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EducationSystem",
                table: "Schools",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$oZCXAowxR3a5lv6ZwM0xsuF7K/eN8VlfewNLir7R/xLiE32sjST06");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EducationSystem",
                table: "Schools");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$uP8u/A/WXANqbpgC9TZ6COBRQI/l5/VeKXI5Yg0Fkue9uWpKkv5.e");
        }
    }
}
