using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStringCollation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "UserEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ResetPasswordCode",
                table: "UserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "BannerUrl",
                table: "UserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "UserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "UserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "MacAddress",
                table: "ServerEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "ServerEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorType",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorField",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "VoterEntityUsername",
                table: "PostVoteEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "PostEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEntityUsername",
                table: "PostEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "TokenType",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RedirectToUserUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PictureUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Locale",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IdToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "GmailAddress",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthUser",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativeUri",
                table: "FileEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativePhysicalPath",
                table: "FileEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "FileEntities",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Field",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ClientMessage",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EmailEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationStatus",
                table: "EmailEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "EnvironmentName",
                table: "ConfigsEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserEntityUsername",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorEntityUsername",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "VoterEntityUsername",
                table: "CommentVoteEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CommentEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEntityUsername",
                table: "CommentEntities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text")
                .OldAnnotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "UserEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "UserEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ResetPasswordCode",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "BannerUrl",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "About",
                table: "UserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SessionEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "MacAddress",
                table: "ServerEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "ServerEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserAgent",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorType",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ErrorField",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "RequestTraceEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "RequestTraceEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "VoterEntityUsername",
                table: "PostVoteEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEntityUsername",
                table: "PostEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "TokenType",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Scope",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RedirectToUserUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Prompt",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "PictureUrl",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Locale",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "IdToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "GmailAddress",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthUser",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AccessToken",
                table: "GoogleUserEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativeUri",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RelativePhysicalPath",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "RealName",
                table: "FileEntities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "FileEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Field",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ClientMessage",
                table: "ErrorEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "EmailEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "ActivationStatus",
                table: "EmailEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "EnvironmentName",
                table: "ConfigsEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserEntityUsername",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CommunityEntityName",
                table: "CommunityMembershipEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorEntityUsername",
                table: "CommunityEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "VoterEntityUsername",
                table: "CommentVoteEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "CommentEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorEntityUsername",
                table: "CommentEntities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive")
                .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive");
        }
    }
}
