using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddCommunityQueryResultEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommunityQueryResultEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Pointer = table.Column<string>(type: "text", nullable: true),
                    LastStart = table.Column<int>(type: "integer", nullable: false),
                    LastEnd = table.Column<int>(type: "integer", nullable: false),
                    LastLimit = table.Column<int>(type: "integer", nullable: false),
                    ReferenceEntityIds = table.Column<List<long>>(type: "bigint[]", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityQueryResultEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityQueryResultEntities_Pointer",
                table: "CommunityQueryResultEntities",
                column: "Pointer",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityQueryResultEntities");
        }
    }
}
