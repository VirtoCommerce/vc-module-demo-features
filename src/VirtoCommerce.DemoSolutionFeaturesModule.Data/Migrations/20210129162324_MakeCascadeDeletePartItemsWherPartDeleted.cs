using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class MakeCascadeDeletePartItemsWherPartDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoProductPartItem_DemoProductPart_ConfiguredProductPartId",
                table: "DemoProductPartItem");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoProductPartItem_DemoProductPart_ConfiguredProductPartId",
                table: "DemoProductPartItem",
                column: "ConfiguredProductPartId",
                principalTable: "DemoProductPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoProductPartItem_DemoProductPart_ConfiguredProductPartId",
                table: "DemoProductPartItem");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoProductPartItem_DemoProductPart_ConfiguredProductPartId",
                table: "DemoProductPartItem",
                column: "ConfiguredProductPartId",
                principalTable: "DemoProductPart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
