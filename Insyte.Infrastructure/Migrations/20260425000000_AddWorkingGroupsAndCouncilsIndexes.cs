using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insyte.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingGroupsAndCouncilsIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Indexes for working_groups table
            migrationBuilder.CreateIndex(
                name: "ix_working_groups_school_id",
                table: "working_groups",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_working_groups_school_id_name",
                table: "working_groups",
                columns: new[] { "school_id", "name" });

            migrationBuilder.CreateIndex(
                name: "ix_working_groups_is_active",
                table: "working_groups",
                column: "is_active");

            // Indexes for working_group_members table
            migrationBuilder.CreateIndex(
                name: "ix_working_group_members_working_group_id",
                table: "working_group_members",
                column: "working_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_working_group_members_user_id",
                table: "working_group_members",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_working_group_members_working_group_id_user_id",
                table: "working_group_members",
                columns: new[] { "working_group_id", "user_id" },
                unique: true);

            // Indexes for councils table
            migrationBuilder.CreateIndex(
                name: "ix_councils_school_id",
                table: "councils",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "ix_councils_school_id_name",
                table: "councils",
                columns: new[] { "school_id", "name" });

            migrationBuilder.CreateIndex(
                name: "ix_councils_is_active",
                table: "councils",
                column: "is_active");

            // Indexes for council_members table
            migrationBuilder.CreateIndex(
                name: "ix_council_members_council_id",
                table: "council_members",
                column: "council_id");

            migrationBuilder.CreateIndex(
                name: "ix_council_members_user_id",
                table: "council_members",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_council_members_council_id_user_id",
                table: "council_members",
                columns: new[] { "council_id", "user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_working_groups_school_id",
                table: "working_groups");

            migrationBuilder.DropIndex(
                name: "ix_working_groups_school_id_name",
                table: "working_groups");

            migrationBuilder.DropIndex(
                name: "ix_working_groups_is_active",
                table: "working_groups");

            migrationBuilder.DropIndex(
                name: "ix_working_group_members_working_group_id",
                table: "working_group_members");

            migrationBuilder.DropIndex(
                name: "ix_working_group_members_user_id",
                table: "working_group_members");

            migrationBuilder.DropIndex(
                name: "ix_working_group_members_working_group_id_user_id",
                table: "working_group_members");

            migrationBuilder.DropIndex(
                name: "ix_councils_school_id",
                table: "councils");

            migrationBuilder.DropIndex(
                name: "ix_councils_school_id_name",
                table: "councils");

            migrationBuilder.DropIndex(
                name: "ix_councils_is_active",
                table: "councils");

            migrationBuilder.DropIndex(
                name: "ix_council_members_council_id",
                table: "council_members");

            migrationBuilder.DropIndex(
                name: "ix_council_members_user_id",
                table: "council_members");

            migrationBuilder.DropIndex(
                name: "ix_council_members_council_id_user_id",
                table: "council_members");
        }
    }
}
