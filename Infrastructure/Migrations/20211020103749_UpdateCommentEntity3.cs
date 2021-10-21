using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class UpdateCommentEntity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<List<long>>(
                name: "ParentCommentEntityIds",
                table: "CommentEntities",
                type: "bigint[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCommentEntityIds",
                table: "CommentEntities");

            migrationBuilder.AddColumn<long>(
                name: "ParentCommentEntityId",
                table: "CommentEntities",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntities_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId",
                principalTable: "CommentEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
