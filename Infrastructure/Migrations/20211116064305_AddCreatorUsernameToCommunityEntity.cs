using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddCreatorUsernameToCommunityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId",
                table: "CommunityEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities");

            migrationBuilder.AddColumn<string>(
                name: "CreatorEntityUsername",
                table: "CommunityEntities",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserEntities_Username",
                table: "UserEntities",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities",
                column: "CreatorEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityUsername",
                table: "CommunityEntities",
                column: "CreatorEntityUsername",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityUsername",
                table: "CommunityEntities",
                column: "CreatorEntityUsername",
                principalTable: "UserEntities",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityUsername",
                table: "CommunityEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserEntities_Username",
                table: "UserEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityUsername",
                table: "CommunityEntities");

            migrationBuilder.DropColumn(
                name: "CreatorEntityUsername",
                table: "CommunityEntities");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities",
                column: "CreatorEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId",
                table: "CommunityEntities",
                column: "CreatorEntityId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
