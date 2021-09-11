using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class UpdateCommentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentCommentEntityId",
                table: "CommentEntities",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntities_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId",
                principalTable: "CommentEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntities_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities");

            migrationBuilder.DropColumn(
                name: "ParentCommentEntityId",
                table: "CommentEntities");
        }
    }
}
