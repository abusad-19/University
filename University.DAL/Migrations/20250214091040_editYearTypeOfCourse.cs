using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.DAL.Migrations
{
    /// <inheritdoc />
    public partial class editYearTypeOfCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearCode",
                table: "CourseTable");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "CourseTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "CourseTable");

            migrationBuilder.AddColumn<int>(
                name: "YearCode",
                table: "CourseTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
