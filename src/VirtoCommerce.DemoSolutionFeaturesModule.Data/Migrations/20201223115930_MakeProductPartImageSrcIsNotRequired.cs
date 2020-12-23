using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class MakeProductPartImageSrcIsNotRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgSrc",
                table: "DemoProductPart",
                maxLength: 2083,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2083)",
                oldMaxLength: 2083);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImgSrc",
                table: "DemoProductPart",
                type: "nvarchar(2083)",
                maxLength: 2083,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2083,
                oldNullable: true);
        }
    }
}
