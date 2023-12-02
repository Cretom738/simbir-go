using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransportType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    AccountRoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_AccountRole_AccountRoleId",
                        column: x => x.AccountRoleId,
                        principalTable: "AccountRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanBeRented = table.Column<bool>(type: "boolean", nullable: false),
                    Model = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Color = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Identifier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    MinutePrice = table.Column<double>(type: "double precision", nullable: true),
                    DayPrice = table.Column<double>(type: "double precision", nullable: true),
                    TransportTypeId = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transports_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transports_TransportType_TransportTypeId",
                        column: x => x.TransportTypeId,
                        principalTable: "TransportType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimeStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PriceOfUnit = table.Column<double>(type: "double precision", nullable: false),
                    FinalPrice = table.Column<double>(type: "double precision", nullable: true),
                    RenterId = table.Column<long>(type: "bigint", nullable: false),
                    RentTypeId = table.Column<int>(type: "integer", nullable: false),
                    TransportId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rents_Accounts_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rents_RentType_RentTypeId",
                        column: x => x.RentTypeId,
                        principalTable: "RentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rents_Transports_TransportId",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AccountRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "RentType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Minutes" },
                    { 2, "Days" }
                });

            migrationBuilder.InsertData(
                table: "TransportType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Car" },
                    { 2, "Bike" },
                    { 3, "Scooter" }
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "AccountRoleId", "Balance", "PasswordHash", "Username" },
                values: new object[] { 1L, 1, 0.0, "$2a$13$02pX6t3i/SewMaRWGOVcGuUMxMxZORBRdoEohdIytu.Hc5xknLKX.", "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountRole_Name",
                table: "AccountRole",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountRoleId",
                table: "Accounts",
                column: "AccountRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Username",
                table: "Accounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rents_RenterId",
                table: "Rents",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_RentTypeId",
                table: "Rents",
                column: "RentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rents_TransportId",
                table: "Rents",
                column: "TransportId");

            migrationBuilder.CreateIndex(
                name: "IX_RentType_Name",
                table: "RentType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_OwnerId",
                table: "Transports",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_TransportTypeId",
                table: "Transports",
                column: "TransportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportType_Name",
                table: "TransportType",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rents");

            migrationBuilder.DropTable(
                name: "RentType");

            migrationBuilder.DropTable(
                name: "Transports");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "TransportType");

            migrationBuilder.DropTable(
                name: "AccountRole");
        }
    }
}
