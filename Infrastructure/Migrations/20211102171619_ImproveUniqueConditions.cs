using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class ImproveUniqueConditions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_PostVoteEntities_VoterEntityId_PostEntityId",
                table: "PostVoteEntities",
                columns: new[] { "VoterEntityId", "PostEntityId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CommunityMembershipEntities_UserEntityId_CommunityEntityId",
                table: "CommunityMembershipEntities",
                columns: new[] { "UserEntityId", "CommunityEntityId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CommentVoteEntities_VoterEntityId_CommentEntityId",
                table: "CommentVoteEntities",
                columns: new[] { "VoterEntityId", "CommentEntityId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_PostVoteEntities_VoterEntityId_PostEntityId",
                table: "PostVoteEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CommunityMembershipEntities_UserEntityId_CommunityEntityId",
                table: "CommunityMembershipEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CommentVoteEntities_VoterEntityId_CommentEntityId",
                table: "CommentVoteEntities");
        }
    }
}
