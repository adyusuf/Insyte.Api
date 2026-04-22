using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolLogoAndCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Schools",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoPath",
                table: "Schools",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Schools",
                type: "double precision",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$Hwx21GDTdokwEvWdsyh20uAbyNdS/q6Z6HxQ2F.eCsT1h1va0LwdW");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "LogoPath",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Schools");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"),
                column: "PasswordHash",
                value: "$2a$11$3.fe/S2Jjgy4En0CRy8r.e2jlGgk6slexOoGtOl56BltFfsZEEmqG");
        }
    }
}
