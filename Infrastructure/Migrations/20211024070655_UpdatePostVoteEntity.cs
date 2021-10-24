using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class UpdatePostVoteEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId",
                table: "PostVoteEntities");

            migrationBuilder.AddColumn<string>(
                name: "VoterEntityUsername",
                table: "PostVoteEntities",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityId_VoterEntityUsername",
                table: "PostVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityUsername",
                table: "PostVoteEntities",
                column: "VoterEntityUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                table: "PostVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                table: "PostVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteEntities_VoterEntityId_VoterEntityUsername",
                table: "PostVoteEntities");

            migrationBuilder.DropIndex(
                name: "IX_PostVoteEntities_VoterEntityUsername",
                table: "PostVoteEntities");

            migrationBuilder.DropColumn(
                name: "VoterEntityUsername",
                table: "PostVoteEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_PostVoteEntities_UserEntities_VoterEntityId",
                table: "PostVoteEntities",
                column: "VoterEntityId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
