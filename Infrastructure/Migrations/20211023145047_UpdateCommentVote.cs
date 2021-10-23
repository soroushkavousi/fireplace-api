using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class UpdateCommentVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId",
                table: "CommentVoteEntities");

            migrationBuilder.AddColumn<string>(
                name: "VoterEntityUsername",
                table: "CommentVoteEntities",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityId_VoterEntityUsername",
                table: "CommentVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityUsername",
                table: "CommentVoteEntities",
                column: "VoterEntityUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                table: "CommentVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                table: "CommentVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentVoteEntities_VoterEntityId_VoterEntityUsername",
                table: "CommentVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentVoteEntities_VoterEntityUsername",
                table: "CommentVoteEntities");

            migrationBuilder.DropColumn(
                name: "VoterEntityUsername",
                table: "CommentVoteEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId",
                table: "CommentVoteEntities",
                column: "VoterEntityId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
