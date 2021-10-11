using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixIndexesOfPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostEntities_AuthorEntityId",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_CommunityEntityId",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityId",
                table: "PostEntities",
                column: "AuthorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities",
                column: "AuthorEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityId",
                table: "PostEntities",
                column: "CommunityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities",
                column: "CommunityEntityName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostEntities_AuthorEntityId",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_CommunityEntityId",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityId",
                table: "PostEntities",
                column: "AuthorEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities",
                column: "AuthorEntityUsername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityId",
                table: "PostEntities",
                column: "CommunityEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities",
                column: "CommunityEntityName",
                unique: true);
        }
    }
}
