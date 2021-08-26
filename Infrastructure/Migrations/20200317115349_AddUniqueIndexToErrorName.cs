using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddUniqueIndexToErrorName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ErrorEntities",
                nullable: false,
                defaultValue: "INTERNAL_SERVER",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "INTERNAL_SERVER");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorEntities_Name",
                table: "ErrorEntities",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ErrorEntities_Name",
                table: "ErrorEntities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ErrorEntities",
                type: "text",
                nullable: true,
                defaultValue: "INTERNAL_SERVER",
                oldClrType: typeof(string),
                oldDefaultValue: "INTERNAL_SERVER");
        }
    }
}
