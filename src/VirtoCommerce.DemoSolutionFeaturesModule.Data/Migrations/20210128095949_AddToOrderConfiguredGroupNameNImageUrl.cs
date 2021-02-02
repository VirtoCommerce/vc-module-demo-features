using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class AddToOrderConfiguredGroupNameNImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DemoOrderConfiguredGroup",
                maxLength: 1028,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DemoOrderConfiguredGroup",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DemoOrderConfiguredGroup");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DemoOrderConfiguredGroup");
        }
    }
}
