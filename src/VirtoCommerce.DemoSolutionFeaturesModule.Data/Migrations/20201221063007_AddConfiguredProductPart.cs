using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class AddConfiguredProductPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Item",
                maxLength: 128,
                nullable: false,
                defaultValue: "DemoItemEntity");

            migrationBuilder.CreateTable(
                name: "DemoProductPart",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ConfiguredProductId = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 1024, nullable: false),
                    ImgSrc = table.Column<string>(maxLength: 2083, nullable: false),
                    DefaultItemId = table.Column<string>(maxLength: 128, nullable: true),
                    Priority = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoProductPart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoProductPart_Item_ConfiguredProductId",
                        column: x => x.ConfiguredProductId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoProductPartItem",
                columns: table => new
                {
                    ConfiguredProductPartId = table.Column<string>(maxLength: 128, nullable: false),
                    ItemId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoProductPartItem", x => new { x.ConfiguredProductPartId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_DemoProductPartItem_DemoProductPart_ConfiguredProductPartId",
                        column: x => x.ConfiguredProductPartId,
                        principalTable: "DemoProductPart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemoProductPartItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoProductPart_ConfiguredProductId",
                table: "DemoProductPart",
                column: "ConfiguredProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoProductPartItem_ItemId",
                table: "DemoProductPartItem",
                column: "ItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoProductPartItem");

            migrationBuilder.DropTable(
                name: "DemoProductPart");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Item");
        }
    }
}
