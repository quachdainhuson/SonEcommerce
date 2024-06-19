using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SonEcommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddOTPInUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OTP",
                table: "AbpUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OTPExpire",
                table: "AbpUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OTP",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "OTPExpire",
                table: "AbpUsers");
        }
    }
}
