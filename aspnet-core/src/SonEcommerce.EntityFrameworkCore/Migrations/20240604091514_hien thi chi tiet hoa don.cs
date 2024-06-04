using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class hienthichitiethoadon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppOrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppOrderItems");
        }
    }
}
