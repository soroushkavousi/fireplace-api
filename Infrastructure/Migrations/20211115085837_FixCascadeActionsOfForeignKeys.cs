using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixCascadeActionsOfForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                table: "CommentEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                table: "CommentEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);


            migrationBuilder.DropForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                table: "CommentVoteEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                table: "CommentVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);


            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembershipEntities_UserEntities_UserEntityId_UserE~",
                table: "CommunityMembershipEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembershipEntities_UserEntities_UserEntityId_UserE~",
                table: "CommunityMembershipEntities",
                columns: new[] { "UserEntityId", "UserEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);


            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembershipEntities_CommunityEntities_CommunityEnti~",
                table: "CommunityMembershipEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembershipEntities_CommunityEntities_CommunityEnti~",
                table: "CommunityMembershipEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" },
                principalTable: "CommunityEntities",
                principalColumns: new[] { "Id", "Name" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);


            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                table: "PostEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                table: "PostEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);


            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                table: "PostEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                table: "PostEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" },
                principalTable: "CommunityEntities",
                principalColumns: new[] { "Id", "Name" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);


            migrationBuilder.DropForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                table: "PostVoteEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                table: "PostVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
