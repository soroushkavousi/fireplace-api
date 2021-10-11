using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddPostEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId",
                table: "PostEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId",
                table: "PostEntities");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEntityUsername",
                table: "PostEntities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommunityEntityName",
                table: "PostEntities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityId_AuthorEntityUsername",
                table: "PostEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities",
                column: "AuthorEntityUsername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityId_CommunityEntityName",
                table: "PostEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" });

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities",
                column: "CommunityEntityName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                table: "PostEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" },
                principalTable: "CommunityEntities",
                principalColumns: new[] { "Id", "Name" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                table: "PostEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                table: "PostEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_AuthorEntityId_AuthorEntityUsername",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_CommunityEntityId_CommunityEntityName",
                table: "PostEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities");

            migrationBuilder.DropColumn(
                name: "AuthorEntityUsername",
                table: "PostEntities");

            migrationBuilder.DropColumn(
                name: "CommunityEntityName",
                table: "PostEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId",
                table: "PostEntities",
                column: "CommunityEntityId",
                principalTable: "CommunityEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId",
                table: "PostEntities",
                column: "AuthorEntityId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
