using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class RemoveErrorEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorEntities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientMessage = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: false, defaultValue: 400),
                    Name = table.Column<string>(type: "text", nullable: false, defaultValue: "INTERNAL_SERVER")
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
    }
}
