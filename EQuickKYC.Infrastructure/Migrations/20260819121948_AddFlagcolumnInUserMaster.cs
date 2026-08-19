using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQuickKYC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagcolumnInUserMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flag",
                table: "UserMasters",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag",
                table: "UserMasters");
        }
    }
}
