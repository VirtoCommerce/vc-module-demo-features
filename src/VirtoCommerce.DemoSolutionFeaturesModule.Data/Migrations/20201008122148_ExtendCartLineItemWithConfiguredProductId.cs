using Microsoft.EntityFrameworkCore.Migrations;
using VirtoCommerce.DemoSolutionFeaturesModule.Data.Models;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class ExtendCartLineItemWithConfiguredProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CartLineItem",
                nullable: false,
                defaultValue: nameof(DemoCartLineItemEntity));

            migrationBuilder.AddColumn<string>(
                name: "ConfiguredProductId",
                table: "CartLineItem",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CartLineItem");

            migrationBuilder.DropColumn(
                name: "ConfiguredProductId",
                table: "CartLineItem");
        }
    }
}
