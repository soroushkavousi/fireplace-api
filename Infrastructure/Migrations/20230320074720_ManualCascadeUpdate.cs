using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManualCascadeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Community - User
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId_CreatorEntit~",
                table: "CommunityEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId_CreatorEntit~",
                table: "CommunityEntities",
                columns: new[] { nameof(CommunityEntity.CreatorEntityId),
                    nameof(CommunityEntity.CreatorEntityUsername) },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // Community Membership - Community
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembershipEntities_CommunityEntities_CommunityEnti~",
                table: "CommunityMembershipEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembershipEntities_CommunityEntities_CommunityEnti~",
                table: "CommunityMembershipEntities",
                columns: new[] { nameof(CommunityMembershipEntity.CommunityEntityId),
                    nameof(CommunityMembershipEntity.CommunityEntityName) },
                principalTable: "CommunityEntities",
                principalColumns: new[] { "Id", "Name" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // Community Membership - User
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMembershipEntities_UserEntities_UserEntityId_UserE~",
                table: "CommunityMembershipEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMembershipEntities_UserEntities_UserEntityId_UserE~",
                table: "CommunityMembershipEntities",
                columns: new[] { nameof(CommunityMembershipEntity.UserEntityId),
                    nameof(CommunityMembershipEntity.UserEntityUsername) },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // Post - Community
            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                table: "PostEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                table: "PostEntities",
                columns: new[] { nameof(PostEntity.CommunityEntityId),
                    nameof(PostEntity.CommunityEntityName) },
                principalTable: "CommunityEntities",
                principalColumns: new[] { "Id", "Name" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // Post - User
            migrationBuilder.DropForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                table: "PostEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                table: "PostEntities",
                columns: new[] { nameof(PostEntity.AuthorEntityId),
                    nameof(PostEntity.AuthorEntityUsername) },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // PostVote - User
            migrationBuilder.DropForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                table: "PostVoteEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                table: "PostVoteEntities",
                columns: new[] { nameof(PostVoteEntity.VoterEntityId),
                    nameof(PostVoteEntity.VoterEntityUsername) },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // Comment - User
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                table: "CommentEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                table: "CommentEntities",
                columns: new[] { nameof(CommentEntity.AuthorEntityId),
                    nameof(CommentEntity.AuthorEntityUsername) },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

            // CommentVote - User
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                table: "CommentVoteEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                table: "CommentVoteEntities",
                columns: new[] { nameof(CommentVoteEntity.VoterEntityId),
                    nameof(CommentVoteEntity.VoterEntityUsername) },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
