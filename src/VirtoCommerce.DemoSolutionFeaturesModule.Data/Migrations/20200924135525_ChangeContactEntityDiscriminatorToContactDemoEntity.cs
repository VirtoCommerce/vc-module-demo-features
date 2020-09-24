using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class ChangeContactEntityDiscriminatorToContactDemoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
 @"UPDATE [Member]
   SET
       [Discriminator] = 'ContactDemoEntity'
 WHERE[Discriminator] = 'ContactEntity'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
