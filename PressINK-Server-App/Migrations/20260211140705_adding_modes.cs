using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PressINK_Server_App.Migrations
{
    /// <inheritdoc />
    public partial class adding_modes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attribute_Search_Mode",
                table: "API_Templates",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attribute_Search_Mode",
                table: "API_Templates");
        }
    }
}
