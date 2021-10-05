using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class UpdateCommunityMembershipEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMemberEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostVoteEntities",
                table: "PostVoteEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostEntities",
                table: "PostEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentVoteEntities",
                table: "CommentVoteEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEntities",
                table: "CommentEntities");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostVoteEntities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentVoteEntities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserEntities_Id_Username",
                table: "UserEntities",
                columns: new[] { "Id", "Username" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostVoteEntities",
                table: "PostVoteEntities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostEntities",
                table: "PostEntities",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CommunityEntities_Id_Name",
                table: "CommunityEntities",
                columns: new[] { "Id", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentVoteEntities",
                table: "CommentVoteEntities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEntities",
                table: "CommentEntities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommunityMembershipEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEntityId = table.Column<long>(type: "bigint", nullable: false),
                    UserEntityName = table.Column<string>(type: "text", nullable: false),
                    CommunityEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CommunityEntityName = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMembershipEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityMembershipEntities_CommunityEntities_CommunityEnti~",
                        columns: x => new { x.CommunityEntityId, x.CommunityEntityName },
                        principalTable: "CommunityEntities",
                        principalColumns: new[] { "Id", "Name" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMembershipEntities_UserEntities_UserEntityId_UserE~",
                        columns: x => new { x.UserEntityId, x.UserEntityName },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId_CommunityEnti~",
                table: "CommunityMembershipEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" });

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
                name: "IX_CommunityMembershipEntities_UserEntityId_UserEntityName",
                table: "CommunityMembershipEntities",
                columns: new[] { "UserEntityId", "UserEntityName" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityName",
                table: "CommunityMembershipEntities",
                column: "UserEntityName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMembershipEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserEntities_Id_Username",
                table: "UserEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostVoteEntities",
                table: "PostVoteEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostEntities",
                table: "PostEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CommunityEntities_Id_Name",
                table: "CommunityEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentVoteEntities",
                table: "CommentVoteEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEntities",
                table: "CommentEntities");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostVoteEntities",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CommunityEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentVoteEntities",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostVoteEntities",
                table: "PostVoteEntities",
                columns: new[] { "VoterEntityId", "PostEntityId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PostEntities_Id",
                table: "PostEntities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostEntities",
                table: "PostEntities",
                columns: new[] { "AuthorEntityId", "CommunityEntityId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentVoteEntities",
                table: "CommentVoteEntities",
                columns: new[] { "VoterEntityId", "CommentEntityId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CommentEntities_Id",
                table: "CommentEntities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEntities",
                table: "CommentEntities",
                columns: new[] { "AuthorEntityId", "PostEntityId" });

            migrationBuilder.CreateTable(
                name: "CommunityMemberEntities",
                columns: table => new
                {
                    UserEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CommunityEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMemberEntities", x => new { x.UserEntityId, x.CommunityEntityId });
                    table.ForeignKey(
                        name: "FK_CommunityMemberEntities_CommunityEntities_CommunityEntityId",
                        column: x => x.CommunityEntityId,
                        principalTable: "CommunityEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMemberEntities_UserEntities_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMemberEntities_CommunityEntityId",
                table: "CommunityMemberEntities",
                column: "CommunityEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMemberEntities_UserEntityId",
                table: "CommunityMemberEntities",
                column: "UserEntityId",
                unique: true);
        }
    }
}
