using FireplaceApi.Core.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Code = table.Column<int>(nullable: false),
                    ClientMessage = table.Column<string>(nullable: true),
                    HttpStatusCode = table.Column<int>(nullable: false, defaultValue: 400)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    RealName = table.Column<string>(nullable: true),
                    RelativeUri = table.Column<string>(nullable: true),
                    RelativePhysicalPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Values = table.Column<Configs>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccessTokenEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEntityId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessTokenEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessTokenEntities_UserEntities_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEntityId = table.Column<long>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ActivationCode = table.Column<long>(nullable: false),
                    ActivationStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailEntities_UserEntities_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionEntities",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserEntityId = table.Column<long>(nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionEntities_UserEntities_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokenEntities_UserEntityId",
                table: "AccessTokenEntities",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessTokenEntities_Value",
                table: "AccessTokenEntities",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailEntities_Address",
                table: "EmailEntities",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailEntities_UserEntityId",
                table: "EmailEntities",
                column: "UserEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorEntities_Code",
                table: "ErrorEntities",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionEntities_UserEntityId",
                table: "SessionEntities",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntities_Username",
                table: "UserEntities",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTokenEntities");

            migrationBuilder.DropTable(
                name: "EmailEntities");

            migrationBuilder.DropTable(
                name: "ErrorEntities");

            migrationBuilder.DropTable(
                name: "FileEntities");

            migrationBuilder.DropTable(
                name: "GlobalEntities");

            migrationBuilder.DropTable(
                name: "SessionEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
