using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAddressPropertiesToAbpUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "AbpUsers",
                newName: "UserAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserAddress",
                table: "AbpUsers",
                newName: "Address");
        }
    }
}
