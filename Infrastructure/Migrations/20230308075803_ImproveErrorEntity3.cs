using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveErrorEntity3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorName",
                table: "RequestTraceEntities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ErrorEntities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorName",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                defaultValue: "INTERNAL_SERVER")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }
    }
}
