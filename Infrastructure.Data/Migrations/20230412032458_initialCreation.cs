using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ownerId = table.Column<string>(type: "text", nullable: false),
                    categoryId = table.Column<string>(type: "text", nullable: false),
                    isPremium = table.Column<bool>(type: "boolean", nullable: false),
                    instructor = table.Column<string>(type: "text", nullable: false),
                    cost = table.Column<double>(type: "double precision", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    purchaseNumber = table.Column<int>(type: "integer", nullable: false),
                    modulesNumber = table.Column<int>(type: "integer", nullable: false),
                    contentVolume = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseId);
                });

            migrationBuilder.CreateTable(
                name: "CoursesModules",
                columns: table => new
                {
                    courseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesModules", x => x.courseId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EachModule",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    courseTitle = table.Column<string>(type: "text", nullable: false),
                    courseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EachModule", x => x.id);
                    table.ForeignKey(
                        name: "FK_EachModule_CoursesModules_courseId",
                        column: x => x.courseId,
                        principalTable: "CoursesModules",
                        principalColumn: "courseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseUrl",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    EachModuleid = table.Column<Guid>(type: "uuid", nullable: false),
                    courseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUrl", x => x.id);
                    table.ForeignKey(
                        name: "FK_CourseUrl_EachModule_EachModuleid",
                        column: x => x.EachModuleid,
                        principalTable: "EachModule",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseUrl_EachModuleid",
                table: "CourseUrl",
                column: "EachModuleid");

            migrationBuilder.CreateIndex(
                name: "IX_EachModule_courseId",
                table: "EachModule",
                column: "courseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CourseUrl");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EachModule");

            migrationBuilder.DropTable(
                name: "CoursesModules");
        }
    }
}
