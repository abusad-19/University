using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addingYearCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearCode",
                table: "StudentTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearCode",
                table: "StudentResultTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearCode",
                table: "CourseTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearCode",
                table: "StudentTable");

            migrationBuilder.DropColumn(
                name: "YearCode",
                table: "StudentResultTable");

            migrationBuilder.DropColumn(
                name: "YearCode",
                table: "CourseTable");
        }
    }
}
