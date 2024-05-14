using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVisibilitySpellColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Visiblity",
                table: "AppProducts",
                newName: "Visibility");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Visibility",
                table: "AppProducts",
                newName: "Visiblity");
        }
    }
}
