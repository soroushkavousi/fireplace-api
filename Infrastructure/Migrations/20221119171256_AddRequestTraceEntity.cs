using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestTraceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestTraceEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Action = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    IP = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Country = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    ErrorName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Url = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    UserAgent = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTraceEntities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestTraceEntities");
        }
    }
}
