using FireplaceApi.Core.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class FixCreatorForeignKeysInCommunityEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityUsername",
                table: "CommunityEntities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserEntities_Username",
                table: "UserEntities");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "UserEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "PostQueryResultEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<GlobalValues>(
                name: "Values",
                table: "GlobalEntities",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(GlobalValues),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ClientMessage",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EmailEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationStatus",
                table: "EmailEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityQueryResultEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityMembershipQueryResultEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorEntityUsername",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommentQueryResultEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CommentEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AccessTokenEntities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityId_CreatorEntityUsername",
                table: "CommunityEntities",
                columns: new[] { "CreatorEntityId", "CreatorEntityUsername" });

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId_CreatorEntit~",
                table: "CommunityEntities",
                columns: new[] { "CreatorEntityId", "CreatorEntityUsername" },
                principalTable: "UserEntities",
                principalColumns: new[] { "Id", "Username" },
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityId_CreatorEntit~",
                table: "CommunityEntities");

            migrationBuilder.DropIndex(
                name: "IX_CommunityEntities_CreatorEntityId_CreatorEntityUsername",
                table: "CommunityEntities");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "SessionEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SessionEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "PostQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<GlobalValues>(
                name: "Values",
                table: "GlobalEntities",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(GlobalValues),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ClientMessage",
                table: "ErrorEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EmailEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationStatus",
                table: "EmailEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityMembershipQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorEntityUsername",
                table: "CommunityEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommentQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CommentEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AccessTokenEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserEntities_Username",
                table: "UserEntities",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityEntities_UserEntities_CreatorEntityUsername",
                table: "CommunityEntities",
                column: "CreatorEntityUsername",
                principalTable: "UserEntities",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade,
                onUpdate: ReferentialAction.Cascade);
        }
    }
}
