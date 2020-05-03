using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kantin.Migrations
{
    public partial class OrganisationManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false),
                    CanAccessMenu = table.Column<bool>(nullable: false),
                    CanAccessOrder = table.Column<bool>(nullable: false),
                    CanAccessStaffMember = table.Column<bool>(nullable: false),
                    CanAccessSettings = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Privileges_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Privileges_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Privileges_AccountId",
                table: "Privileges",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Privileges_OrganisationId",
                table: "Privileges",
                column: "OrganisationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Privileges");
        }
    }
}
