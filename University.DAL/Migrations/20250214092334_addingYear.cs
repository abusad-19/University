using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addingYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearCode",
                table: "StudentTable");

            migrationBuilder.DropColumn(
                name: "YearCode",
                table: "StudentResultTable");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "StudentTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Mark",
                table: "StudentResultTable",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<float>(
                name: "GPA",
                table: "StudentResultTable",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "StudentResultTable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "StudentTable");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "StudentResultTable");

            migrationBuilder.AddColumn<int>(
                name: "YearCode",
                table: "StudentTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Mark",
                table: "StudentResultTable",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "GPA",
                table: "StudentResultTable",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearCode",
                table: "StudentResultTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
