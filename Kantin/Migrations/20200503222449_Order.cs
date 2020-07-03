using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kantin.Migrations
{
    public partial class Order : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    OrderTotal = table.Column<double>(nullable: false),
                    OrderDateTime = table.Column<DateTime>(nullable: false),
                    OrderStatus = table.Column<int>(nullable: false),
                    TransactionStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false),
                    MenuItemId = table.Column<Guid>(nullable: false),
                    MenuItemName = table.Column<string>(nullable: true),
                    MenuItemPrice = table.Column<double>(nullable: false),
                    MenuItemAmount = table.Column<int>(nullable: false),
                    MenuItemTotal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderAddOns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    UpdatedDateUTC = table.Column<DateTime>(nullable: false),
                    OrderItemId = table.Column<Guid>(nullable: false),
                    AddOnItemId = table.Column<Guid>(nullable: false),
                    AddOnItemName = table.Column<string>(nullable: true),
                    AddOnItemPrice = table.Column<double>(nullable: false),
                    AddOnItemAmount = table.Column<int>(nullable: false),
                    AddOnItemTotal = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAddOns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderAddOns_AddOnItems_AddOnItemId",
                        column: x => x.AddOnItemId,
                        principalTable: "AddOnItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderAddOns_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddOns_AddOnItemId",
                table: "OrderAddOns",
                column: "AddOnItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddOns_OrderItemId",
                table: "OrderAddOns",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AccountId",
                table: "Orders",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderAddOns");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
