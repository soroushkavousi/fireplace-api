using FireplaceApi.Infrastructure.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddConfigsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigsEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EnvironmentName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Data = table.Column<ConfigsEntityData>(type: "jsonb", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigsEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigsEntities_EnvironmentName",
                table: "ConfigsEntities",
                column: "EnvironmentName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigsEntities");
        }
    }
}
