using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class AddToCartConfiguredGroupNameNImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DemoCartConfiguredGroup",
                maxLength: 1028,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DemoCartConfiguredGroup",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DemoCartConfiguredGroup");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DemoCartConfiguredGroup");
        }
    }
}
