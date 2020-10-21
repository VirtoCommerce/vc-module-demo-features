using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class RollbackCartLineItemExtension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Cart]");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "CartLineItem");

            migrationBuilder.DropColumn(
                name: "ConfiguredProductId",
                table: "CartLineItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "CartLineItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConfiguredProductId",
                table: "CartLineItem",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }
    }
}
