using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class AddDemoOrderConfiguredGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfiguredGroupId",
                table: "OrderLineItem",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "OrderLineItem",
                maxLength: 128,
                nullable: false,
                defaultValue: "DemoOrderLineItemEntity");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CustomerOrder",
                maxLength: 128,
                nullable: false,
                defaultValue: "DemoCustomerOrderEntity");

            migrationBuilder.CreateTable(
                name: "DemoOrderConfiguredGroup",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    CustomerOrderId = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false),
                    Price = table.Column<decimal>(type: "Money", nullable: false),
                    PriceWithTax = table.Column<decimal>(type: "Money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoOrderConfiguredGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoOrderConfiguredGroup_CustomerOrder_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "CustomerOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLineItem_ConfiguredGroupId",
                table: "OrderLineItem",
                column: "ConfiguredGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoOrderConfiguredGroup_CustomerOrderId",
                table: "DemoOrderConfiguredGroup",
                column: "CustomerOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderLineItem_DemoOrderConfiguredGroup_ConfiguredGroupId",
                table: "OrderLineItem",
                column: "ConfiguredGroupId",
                principalTable: "DemoOrderConfiguredGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderLineItem_DemoOrderConfiguredGroup_ConfiguredGroupId",
                table: "OrderLineItem");

            migrationBuilder.DropTable(
                name: "DemoOrderConfiguredGroup");

            migrationBuilder.DropIndex(
                name: "IX_OrderLineItem_ConfiguredGroupId",
                table: "OrderLineItem");

            migrationBuilder.DropColumn(
                name: "ConfiguredGroupId",
                table: "OrderLineItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "OrderLineItem");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CustomerOrder");
        }
    }
}
