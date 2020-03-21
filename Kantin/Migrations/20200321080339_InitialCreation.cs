using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kantin.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    ExpiryDateUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Fullname = table.Column<string>(maxLength: 200, nullable: false),
                    Username = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsArchived = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddOnItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOnItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddOnItems_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    Available = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    SubTitle = table.Column<string>(nullable: true),
                    Available = table.Column<bool>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagGroups_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    AccountId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuAddOnItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    MenuItemId = table.Column<Guid>(nullable: false),
                    AddOnItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuAddOnItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuAddOnItems_AddOnItems_AddOnItemId",
                        column: x => x.AddOnItemId,
                        principalTable: "AddOnItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuAddOnItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemsOnMenus",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    MenuItemId = table.Column<Guid>(nullable: false),
                    MenuId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemsOnMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemsOnMenus_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuItemsOnMenus_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Subtitle = table.Column<string>(nullable: true),
                    TagGroupId = table.Column<Guid>(nullable: false),
                    OrganisationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagValues_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TagValues_TagGroups_TagGroupId",
                        column: x => x.TagGroupId,
                        principalTable: "TagGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_OrganisationId",
                table: "Accounts",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOnItems_OrganisationId",
                table: "AddOnItems",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuAddOnItems_AddOnItemId",
                table: "MenuAddOnItems",
                column: "AddOnItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuAddOnItems_MenuItemId",
                table: "MenuAddOnItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_OrganisationId",
                table: "MenuItems",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemsOnMenus_MenuId",
                table: "MenuItemsOnMenus",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemsOnMenus_MenuItemId",
                table: "MenuItemsOnMenus",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_OrganisationId",
                table: "Menus",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AccountId",
                table: "Sessions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TagGroups_OrganisationId",
                table: "TagGroups",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_TagValues_OrganisationId",
                table: "TagValues",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_TagValues_TagGroupId",
                table: "TagValues",
                column: "TagGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuAddOnItems");

            migrationBuilder.DropTable(
                name: "MenuItemsOnMenus");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "TagValues");

            migrationBuilder.DropTable(
                name: "AddOnItems");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "TagGroups");

            migrationBuilder.DropTable(
                name: "Organisations");
        }
    }
}
