using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class doubletolong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Cost",
                table: "Courses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                table: "Courses",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
