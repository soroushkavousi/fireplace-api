using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class ChangeDefaultCollationOfColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "SessionEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SessionEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEntityUsername",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "TokenType",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RedirectToUserUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PictureUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Locale",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IdToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "GmailAddress",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthUser",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativeUri",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativePhysicalPath",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                defaultValue: "INTERNAL_SERVER",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "INTERNAL_SERVER")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ClientMessage",
                table: "ErrorEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EmailEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationStatus",
                table: "EmailEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityMembershipQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserEntityName",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CommentEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AccessTokenEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:CollationDefinition:case_insensitive", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "SessionEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SessionEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEntityUsername",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "TokenType",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RedirectToUserUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PictureUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Locale",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IdToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "GmailAddress",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthUser",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativeUri",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativePhysicalPath",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                defaultValue: "INTERNAL_SERVER",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "INTERNAL_SERVER")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ClientMessage",
                table: "ErrorEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EmailEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationStatus",
                table: "EmailEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Pointer",
                table: "CommunityMembershipQueryResultEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserEntityName",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CommentEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "AccessTokenEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }
    }
}
