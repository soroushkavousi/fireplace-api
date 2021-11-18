using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class ImproveUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "UserEntities");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "UserEntities",
                newName: "DisplayName");

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "UserEntities",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserEntities",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AddColumn<string>(
                name: "BannerUrl",
                table: "UserEntities",
                type: "text",
                nullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "UserEntities");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserEntities");

            migrationBuilder.DropColumn(
                name: "BannerUrl",
                table: "UserEntities");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "UserEntities",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "UserEntities",
                type: "text",
                nullable: false,
                defaultValue: "")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }
    }
}
