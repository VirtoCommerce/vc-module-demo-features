using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VirtoCommerce.DemoSolutionFeaturesModule.Data.Migrations
{
    public partial class AddTaggedMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemoTaggedMember",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoTaggedMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoTaggedMember_Member_Id",
                        column: x => x.Id,
                        principalTable: "Member",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoMemberTag",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 128, nullable: false),
                    Tag = table.Column<string>(maxLength: 128, nullable: false),
                    TaggedMemberId = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoMemberTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoMemberTag_DemoTaggedMember_TaggedMemberId",
                        column: x => x.TaggedMemberId,
                        principalTable: "DemoTaggedMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemoMemberTag_TaggedMemberId",
                table: "DemoMemberTag",
                column: "TaggedMemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoMemberTag");

            migrationBuilder.DropTable(
                name: "DemoTaggedMember");
        }
    }
}
