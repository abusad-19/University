using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserType_UserCode_toUserClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "UserTable",
                newName: "UserType");

            migrationBuilder.AddColumn<int>(
                name: "UserCode",
                table: "UserTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "UserTable");

            migrationBuilder.RenameColumn(
                name: "UserType",
                table: "UserTable",
                newName: "UserEmail");
        }
    }
}
