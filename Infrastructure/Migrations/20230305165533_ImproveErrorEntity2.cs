using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveErrorEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorCategory",
                table: "RequestTraceEntities",
                newName: "ErrorType");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "ErrorEntities",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ErrorType",
                table: "RequestTraceEntities",
                newName: "ErrorCategory");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "ErrorEntities",
                newName: "Category");
        }
    }
}
