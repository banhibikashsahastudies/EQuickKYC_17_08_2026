using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EQuickKYC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "PanApplicationSeq",
                startValue: 50000L);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    State = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AadhaarNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PanNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    VoterNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DrivingLicenseNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                });

            migrationBuilder.CreateTable(
                name: "EmailOTPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailOTPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileOTPs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileOTPs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationMasters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserMasterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateOnly>(type: "date", nullable: true),
                    ApplicationNumber = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR PanApplicationSeq"),
                    ApplicationPrefix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMasters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MobileOTPId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailOTPId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MobileVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsMobileVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IFSCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MICRCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Users_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AddressId",
                table: "Banks",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AddressId",
                table: "Users",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CardId",
                table: "Users",
                column: "CardId",
                unique: true,
                filter: "[CardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Mobile",
                table: "Users",
                column: "Mobile",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "EmailOTPs");

            migrationBuilder.DropTable(
                name: "MobileOTPs");

            migrationBuilder.DropTable(
                name: "RegistrationMasters");

            migrationBuilder.DropTable(
                name: "UserMasters");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropSequence(
                name: "PanApplicationSeq");
        }
    }
}
