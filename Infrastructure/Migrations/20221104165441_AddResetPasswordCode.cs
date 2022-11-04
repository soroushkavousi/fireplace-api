using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddResetPasswordCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordCode",
                table: "UserEntities",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordCode",
                table: "UserEntities");
        }
    }
}
