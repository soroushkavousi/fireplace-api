using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddCommunityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityEntities_UserEntities_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityMemberEntities",
                columns: table => new
                {
                    UserEntityId = table.Column<long>(type: "bigint", nullable: false),
                    CommunityEntityId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
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
                name: "IX_CommunityEntities_CreatorId",
                table: "CommunityEntities",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_Name",
                table: "CommunityEntities",
                column: "Name",
                unique: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMemberEntities");

            migrationBuilder.DropTable(
                name: "CommunityEntities");
        }
    }
}
