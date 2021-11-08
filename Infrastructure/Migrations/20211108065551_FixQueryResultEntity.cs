using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixQueryResultEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "PostQueryResultEntities",
                type: "numeric[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "CommunityQueryResultEntities",
                type: "numeric[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "CommunityMembershipQueryResultEntities",
                type: "numeric[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "CommentQueryResultEntities",
                type: "numeric[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric(20,0)[]",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "PostQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "CommunityQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "CommunityMembershipQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal[]>(
                name: "ReferenceEntityIds",
                table: "CommentQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(decimal[]),
                oldType: "numeric[]",
                oldNullable: true);
        }
    }
}
