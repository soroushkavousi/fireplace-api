using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class UpdateCommentEntity2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteEntities_PostEntityId",
                table: "PostVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteEntities_VoterEntityId",
                table: "PostVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityName",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_UserEntityName",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentVoteEntities_CommentEntityId",
                table: "CommentVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentVoteEntities_VoterEntityId",
                table: "CommentVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_AuthorEntityId",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_PostEntityId",
                table: "CommentEntities");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEntityUsername",
                table: "CommentEntities",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_PostEntityId",
                table: "PostVoteEntities",
                column: "PostEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityId",
                table: "PostVoteEntities",
                column: "VoterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityName",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId",
                table: "CommunityMembershipEntities",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityName",
                table: "CommunityMembershipEntities",
                column: "UserEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_CommentEntityId",
                table: "CommentVoteEntities",
                column: "CommentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityId",
                table: "CommentVoteEntities",
                column: "VoterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityId",
                table: "CommentEntities",
                column: "AuthorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityId_AuthorEntityUsername",
                table: "CommentEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityUsername",
                table: "CommentEntities",
                column: "AuthorEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_PostEntityId",
                table: "CommentEntities",
                column: "PostEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                table: "CommentEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteEntities_PostEntityId",
                table: "PostVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteEntities_VoterEntityId",
                table: "PostVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityName",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityMembershipEntities_UserEntityName",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentVoteEntities_CommentEntityId",
                table: "CommentVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentVoteEntities_VoterEntityId",
                table: "CommentVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_AuthorEntityId",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_AuthorEntityId_AuthorEntityUsername",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_AuthorEntityUsername",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_PostEntityId",
                table: "CommentEntities");

            migrationBuilder.DropColumn(
                name: "AuthorEntityUsername",
                table: "CommentEntities");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_PostEntityId",
                table: "PostVoteEntities",
                column: "PostEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityId",
                table: "PostVoteEntities",
                column: "VoterEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityName",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId",
                table: "CommunityMembershipEntities",
                column: "UserEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityName",
                table: "CommunityMembershipEntities",
                column: "UserEntityName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_CommentEntityId",
                table: "CommentVoteEntities",
                column: "CommentEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityId",
                table: "CommentVoteEntities",
                column: "VoterEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityId",
                table: "CommentEntities",
                column: "AuthorEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_PostEntityId",
                table: "CommentEntities",
                column: "PostEntityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId",
                table: "CommentEntities",
                column: "AuthorEntityId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
