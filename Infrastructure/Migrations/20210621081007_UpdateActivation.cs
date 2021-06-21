using Microsoft.EntityFrameworkCore.Migrations;

namespace GamingCommunityApi.Infrastructure.Migrations
{
    public partial class UpdateActivation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActivationCode",
                table: "EmailEntities",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ActivationCode",
                table: "EmailEntities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
