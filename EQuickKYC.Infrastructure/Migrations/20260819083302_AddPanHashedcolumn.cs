using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQuickKYC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPanHashedcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PanNoHash",
                table: "RegistrationMasters",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PanNoHash",
                table: "RegistrationMasters");
        }
    }
}
