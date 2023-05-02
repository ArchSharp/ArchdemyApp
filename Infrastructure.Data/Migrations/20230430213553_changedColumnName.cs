using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Topic",
                newName: "TopicId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CoursesModules",
                newName: "CourseModuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TopicId",
                table: "Topic",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "CourseModuleId",
                table: "CoursesModules",
                newName: "Id");
        }
    }
}
