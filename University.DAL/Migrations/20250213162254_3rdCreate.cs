using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _3rdCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Credit",
                table: "CourseTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StudentResultTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCredit = table.Column<int>(type: "int", nullable: false),
                    Mark = table.Column<int>(type: "int", nullable: false),
                    GPA = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentResultTable", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentResultTable");

            migrationBuilder.DropColumn(
                name: "Credit",
                table: "CourseTable");
        }
    }
}
