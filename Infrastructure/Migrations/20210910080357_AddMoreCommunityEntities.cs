using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddMoreCommunityEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorId",
                table: "CommunityEntities");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "CommunityEntities",
                newName: "CreatorEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityEntities_CreatorId",
                table: "CommunityEntities",
                newName: "IX_CommunityEntities_CreatorEntityId");

            migrationBuilder.CreateTable(
                name: "PostEntities",
                columns: table => new
                {
                    AuthorEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CommunityEntityId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Vote = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostEntities", x => new { x.AuthorEntityId, x.CommunityEntityId });
                    table.UniqueConstraint("AK_PostEntities_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostEntities_CommunityEntities_CommunityEntityId",
                        column: x => x.CommunityEntityId,
                        principalTable: "CommunityEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEntities_UserEntities_AuthorEntityId",
                        column: x => x.AuthorEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentEntities",
                columns: table => new
                {
                    AuthorEntityId = table.Column<long>(type: "bigint", nullable: false),
                    PostEntityId = table.Column<long>(type: "bigint", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Vote = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentEntities", x => new { x.AuthorEntityId, x.PostEntityId });
                    table.UniqueConstraint("AK_CommentEntities_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentEntities_PostEntities_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "PostEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentEntities_UserEntities_AuthorEntityId",
                        column: x => x.AuthorEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostVoteEntities",
                columns: table => new
                {
                    VoterEntityId = table.Column<long>(type: "bigint", nullable: false),
                    PostEntityId = table.Column<long>(type: "bigint", nullable: false),
                    IsUp = table.Column<bool>(type: "boolean", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostVoteEntities", x => new { x.VoterEntityId, x.PostEntityId });
                    table.ForeignKey(
                        name: "FK_PostVoteEntities_PostEntities_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "PostEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostVoteEntities_UserEntities_VoterEntityId",
                        column: x => x.VoterEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentVoteEntities",
                columns: table => new
                {
                    VoterEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CommentEntityId = table.Column<long>(type: "bigint", nullable: false),
                    IsUp = table.Column<bool>(type: "boolean", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentVoteEntities", x => new { x.VoterEntityId, x.CommentEntityId });
                    table.ForeignKey(
                        name: "FK_CommentVoteEntities_CommentEntities_CommentEntityId",
                        column: x => x.CommentEntityId,
                        principalTable: "CommentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentVoteEntities_UserEntities_VoterEntityId",
                        column: x => x.VoterEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityId",
                table: "CommentEntities",
                column: "AuthorEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_PostEntityId",
                table: "CommentEntities",
                column: "PostEntityId",
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
                name: "IX_PostEntities_AuthorEntityId",
                table: "PostEntities",
                column: "AuthorEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityId",
                table: "PostEntities",
                column: "CommunityEntityId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId",
                table: "CommunityEntities",
                column: "CreatorEntityId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId",
                table: "CommunityEntities");

            migrationBuilder.DropTable(
                name: "CommentVoteEntities");

            migrationBuilder.DropTable(
                name: "PostVoteEntities");

            migrationBuilder.DropTable(
                name: "CommentEntities");

            migrationBuilder.DropTable(
                name: "PostEntities");

            migrationBuilder.RenameColumn(
                name: "CreatorEntityId",
                table: "CommunityEntities",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities",
                newName: "IX_CommunityEntities_CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorId",
                table: "CommunityEntities",
                column: "CreatorId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
