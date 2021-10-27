using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddPostQueryResultEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostQueryResultEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Pointer = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    LastStart = table.Column<int>(type: "integer", nullable: false),
                    LastEnd = table.Column<int>(type: "integer", nullable: false),
                    LastLimit = table.Column<int>(type: "integer", nullable: false),
                    LastPage = table.Column<int>(type: "integer", nullable: false),
                    ReferenceEntityIds = table.Column<List<long>>(type: "bigint[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostQueryResultEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostQueryResultEntities_Pointer",
                table: "PostQueryResultEntities",
                column: "Pointer",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostQueryResultEntities");
        }
    }
}
