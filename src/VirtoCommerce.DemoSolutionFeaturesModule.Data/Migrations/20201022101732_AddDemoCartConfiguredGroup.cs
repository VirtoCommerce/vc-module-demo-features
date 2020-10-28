using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class AddDemoCartConfiguredGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CartLineItem",
                maxLength: 128,
                nullable: false,
                defaultValue: "DemoCartLineItemEntity");

            migrationBuilder.AddColumn<string>(
                name: "ConfiguredGroupId",
                table: "CartLineItem",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Cart",
                maxLength: 128,
                nullable: false,
                defaultValue: "DemoShoppingCartEntity");

            migrationBuilder.CreateTable(
                name: "DemoCartConfiguredGroup",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    ShoppingCartId = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    ListPrice = table.Column<decimal>(type: "Money", nullable: false),
                    ListPriceWithTax = table.Column<decimal>(type: "Money", nullable: false),
                    SalePrice = table.Column<decimal>(type: "Money", nullable: false),
                    SalePriceWithTax = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoCartConfiguredGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoCartConfiguredGroup_Cart_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartLineItem_ConfiguredGroupId",
                table: "CartLineItem",
                column: "ConfiguredGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoCartConfiguredGroup_ShoppingCartId",
                table: "DemoCartConfiguredGroup",
                column: "ShoppingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartLineItem_DemoCartConfiguredGroup_ConfiguredGroupId",
                table: "CartLineItem",
                column: "ConfiguredGroupId",
                principalTable: "DemoCartConfiguredGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartLineItem_DemoCartConfiguredGroup_ConfiguredGroupId",
                table: "CartLineItem");

            migrationBuilder.DropTable(
                name: "DemoCartConfiguredGroup");

            migrationBuilder.DropIndex(
                name: "IX_CartLineItem_ConfiguredGroupId",
                table: "CartLineItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CartLineItem");

            migrationBuilder.DropColumn(
                name: "ConfiguredGroupId",
                table: "CartLineItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Cart");
        }
    }
}
