using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQuickKYC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingMobileEmailOtpHashIncluded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MobileHash",
                table: "MobileOTPs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OTPHash",
                table: "MobileOTPs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HashEmail",
                table: "EmailOTPs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HashOTP",
                table: "EmailOTPs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileHash",
                table: "MobileOTPs");

            migrationBuilder.DropColumn(
                name: "OTPHash",
                table: "MobileOTPs");

            migrationBuilder.DropColumn(
                name: "HashEmail",
                table: "EmailOTPs");

            migrationBuilder.DropColumn(
                name: "HashOTP",
                table: "EmailOTPs");
        }
    }
}
