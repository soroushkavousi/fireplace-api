using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddCommunityMembershipQueryResultEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityMembershipQueryResultEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Pointer = table.Column<string>(type: "text", nullable: true),
                    LastStart = table.Column<int>(type: "integer", nullable: false),
                    LastEnd = table.Column<int>(type: "integer", nullable: false),
                    LastLimit = table.Column<int>(type: "integer", nullable: false),
                    LastPage = table.Column<int>(type: "integer", nullable: false),
                    ReferenceEntityIds = table.Column<List<long>>(type: "bigint[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMembershipQueryResultEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipQueryResultEntities_Pointer",
                table: "CommunityMembershipQueryResultEntities",
                column: "Pointer",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMembershipQueryResultEntities");
        }
    }
}
