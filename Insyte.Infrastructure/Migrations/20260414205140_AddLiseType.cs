using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLiseType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LiseType",
                table: "Schools",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$uP8u/A/WXANqbpgC9TZ6COBRQI/l5/VeKXI5Yg0Fkue9uWpKkv5.e");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiseType",
                table: "Schools");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$Jfyh/H4FnR7ohZG9nw1i/e8abZEX.SQjnnVXwD5wxovbgzNoPkVju");
        }
    }
}
