using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GamingCommunityApi.Infrastructure.Migrations
{
    public partial class AddErrorEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false, defaultValue: "INTERNAL_SERVER"),
                    Code = table.Column<int>(nullable: false),
                    ClientMessage = table.Column<string>(nullable: true),
                    HttpStatusCode = table.Column<int>(nullable: false, defaultValue: 400)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorEntities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorEntities_Code",
                table: "ErrorEntities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorEntities_Name",
                table: "ErrorEntities",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorEntities");
        }
    }
}
