using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddGoogleUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoogleUserEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEntityId = table.Column<long>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    AccessToken = table.Column<string>(nullable: true),
                    TokenType = table.Column<string>(nullable: true),
                    AccessTokenExpiresInSeconds = table.Column<long>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: true),
                    Scope = table.Column<string>(nullable: true),
                    IdToken = table.Column<string>(nullable: true),
                    AccessTokenIssuedTime = table.Column<DateTime>(nullable: false),
                    GmailAddress = table.Column<string>(nullable: true),
                    GmailVerified = table.Column<bool>(nullable: false),
                    GmailIssuedTimeInSeconds = table.Column<long>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Locale = table.Column<string>(nullable: true),
                    PictureUrl = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    AuthUser = table.Column<string>(nullable: true),
                    Prompt = table.Column<string>(nullable: true),
                    RedirectToUserUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleUserEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoogleUserEntities_UserEntities_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUserEntities_GmailAddress",
                table: "GoogleUserEntities",
                column: "GmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUserEntities_UserEntityId",
                table: "GoogleUserEntities",
                column: "UserEntityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleUserEntities");
        }
    }
}
