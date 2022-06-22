using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixCommunityUniqueConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityUsername",
                table: "CommunityEntities");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities",
                column: "CreatorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityUsername",
                table: "CommunityEntities",
                column: "CreatorEntityUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityUsername",
                table: "CommunityEntities");

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
        }
    }
}
