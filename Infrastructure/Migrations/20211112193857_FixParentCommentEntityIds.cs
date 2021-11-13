using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixParentCommentEntityIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal[]>(
                name: "ParentCommentEntityIds",
                table: "CommentEntities",
                type: "numeric[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric(20,0)[]",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal[]>(
                name: "ParentCommentEntityIds",
                table: "CommentEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric[]",
                oldNullable: true);
        }
    }
}
