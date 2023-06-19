using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTokenEntities");

            migrationBuilder.AddColumn<List<string>>(
                name: "Roles",
                table: "UserEntities",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Roles",
                table: "UserEntities");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "SessionEntities");

            migrationBuilder.CreateTable(
                name: "AccessTokenEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTokenEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessTokenEntities_UserEntities_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokenEntities_UserEntityId",
                table: "AccessTokenEntities",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokenEntities_Value",
                table: "AccessTokenEntities",
                column: "Value",
                unique: true);
        }
    }
}
