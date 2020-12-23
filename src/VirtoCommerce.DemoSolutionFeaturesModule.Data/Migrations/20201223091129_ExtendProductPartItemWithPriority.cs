using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class ExtendProductPartItemWithPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "DemoProductPartItem",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "DemoProductPartItem");
        }
    }
}
