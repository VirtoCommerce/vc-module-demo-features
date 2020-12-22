using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class ExtendProductPartWithDescriptionAndOtherFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DemoProductPart",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "DemoProductPart",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxQuantity",
                table: "DemoProductPart",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinQuantity",
                table: "DemoProductPart",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DemoProductPart");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "DemoProductPart");

            migrationBuilder.DropColumn(
                name: "MaxQuantity",
                table: "DemoProductPart");

            migrationBuilder.DropColumn(
                name: "MinQuantity",
                table: "DemoProductPart");
        }
    }
}
