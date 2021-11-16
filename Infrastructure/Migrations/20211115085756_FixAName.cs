using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixAName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEntityName",
                table: "CommunityMembershipEntities",
                newName: "UserEntityUsername");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembershipEntities_UserEntityName",
                table: "CommunityMembershipEntities",
                newName: "IX_CommunityMembershipEntities_UserEntityUsername");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId_UserEntityName",
                table: "CommunityMembershipEntities",
                newName: "IX_CommunityMembershipEntities_UserEntityId_UserEntityUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserEntityUsername",
                table: "CommunityMembershipEntities",
                newName: "UserEntityName");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembershipEntities_UserEntityUsername",
                table: "CommunityMembershipEntities",
                newName: "IX_CommunityMembershipEntities_UserEntityName");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId_UserEntityUsername",
                table: "CommunityMembershipEntities",
                newName: "IX_CommunityMembershipEntities_UserEntityId_UserEntityName");
        }
    }
}
