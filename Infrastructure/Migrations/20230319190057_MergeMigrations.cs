using FireplaceApi.Infrastructure.ValueObjects;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive", "en-u-ks-primary,en-u-ks-primary,icu,False");

            migrationBuilder.CreateTable(
                name: "ConfigsEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    EnvironmentName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Data = table.Column<ConfigsEntityData>(type: "jsonb", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigsEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Field = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    ClientMessage = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: false, defaultValue: 400),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    RealName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    RelativeUri = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    RelativePhysicalPath = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestTraceEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Action = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    IP = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Country = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Duration = table.Column<long>(type: "bigint", nullable: false),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    ErrorType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    ErrorField = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Url = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    UserAgent = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTraceEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    State = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    DisplayName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    About = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    BannerUrl = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    PasswordHash = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    ResetPasswordCode = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                    table.UniqueConstraint("AK_UserEntities_Id_Username", x => new { x.Id, x.Username });
                });

            migrationBuilder.CreateTable(
                name: "AccessTokenEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "CommunityEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreatorEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CreatorEntityUsername = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityEntities", x => x.Id);
                    table.UniqueConstraint("AK_CommunityEntities_Id_Name", x => new { x.Id, x.Name });
                    table.ForeignKey(
                        name: "FK_CommunityEntities_UserEntities_CreatorEntityId_CreatorEntit~",
                        columns: x => new { x.CreatorEntityId, x.CreatorEntityUsername },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    ActivationStatus = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    ActivationCode = table.Column<int>(type: "integer", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "GoogleUserEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    AccessToken = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    TokenType = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    AccessTokenExpiresInSeconds = table.Column<long>(type: "bigint", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Scope = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    IdToken = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    AccessTokenIssuedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GmailAddress = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    GmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    GmailIssuedTimeInSeconds = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    FirstName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    LastName = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Locale = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    PictureUrl = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    State = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    AuthUser = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Prompt = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    RedirectToUserUrl = table.Column<string>(type: "text", nullable: true)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "SessionEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    State = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "CommunityMembershipEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserEntityUsername = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CommunityEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CommunityEntityName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMembershipEntities", x => x.Id);
                    table.UniqueConstraint("AK_CommunityMembershipEntities_UserEntityId_CommunityEntityId", x => new { x.UserEntityId, x.CommunityEntityId });
                    table.ForeignKey(
                        name: "FK_CommunityMembershipEntities_CommunityEntities_CommunityEnti~",
                        columns: x => new { x.CommunityEntityId, x.CommunityEntityName },
                        principalTable: "CommunityEntities",
                        principalColumns: new[] { "Id", "Name" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMembershipEntities_UserEntities_UserEntityId_UserE~",
                        columns: x => new { x.UserEntityId, x.UserEntityUsername },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AuthorEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AuthorEntityUsername = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CommunityEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CommunityEntityName = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    Vote = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostEntities_CommunityEntities_CommunityEntityId_CommunityE~",
                        columns: x => new { x.CommunityEntityId, x.CommunityEntityName },
                        principalTable: "CommunityEntities",
                        principalColumns: new[] { "Id", "Name" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostEntities_UserEntities_AuthorEntityId_AuthorEntityUserna~",
                        columns: x => new { x.AuthorEntityId, x.AuthorEntityUsername },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AuthorEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    AuthorEntityUsername = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    PostEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ParentCommentEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: true),
                    Vote = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentEntities_CommentEntities_ParentCommentEntityId",
                        column: x => x.ParentCommentEntityId,
                        principalTable: "CommentEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentEntities_PostEntities_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "PostEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentEntities_UserEntities_AuthorEntityId_AuthorEntityUse~",
                        columns: x => new { x.AuthorEntityId, x.AuthorEntityUsername },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostVoteEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    VoterEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    VoterEntityUsername = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    PostEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IsUp = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostVoteEntities", x => x.Id);
                    table.UniqueConstraint("AK_PostVoteEntities_VoterEntityId_PostEntityId", x => new { x.VoterEntityId, x.PostEntityId });
                    table.ForeignKey(
                        name: "FK_PostVoteEntities_PostEntities_PostEntityId",
                        column: x => x.PostEntityId,
                        principalTable: "PostEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostVoteEntities_UserEntities_VoterEntityId_VoterEntityUser~",
                        columns: x => new { x.VoterEntityId, x.VoterEntityUsername },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentVoteEntities",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    VoterEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    VoterEntityUsername = table.Column<string>(type: "text", nullable: false)
                        .Annotation("Npgsql:DefaultColumnCollation", "case_insensitive"),
                    CommentEntityId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    IsUp = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentVoteEntities", x => x.Id);
                    table.UniqueConstraint("AK_CommentVoteEntities_VoterEntityId_CommentEntityId", x => new { x.VoterEntityId, x.CommentEntityId });
                    table.ForeignKey(
                        name: "FK_CommentVoteEntities_CommentEntities_CommentEntityId",
                        column: x => x.CommentEntityId,
                        principalTable: "CommentEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentVoteEntities_UserEntities_VoterEntityId_VoterEntityU~",
                        columns: x => new { x.VoterEntityId, x.VoterEntityUsername },
                        principalTable: "UserEntities",
                        principalColumns: new[] { "Id", "Username" },
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
                name: "IX_CommentEntities_AuthorEntityId",
                table: "CommentEntities",
                column: "AuthorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityId_AuthorEntityUsername",
                table: "CommentEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_AuthorEntityUsername",
                table: "CommentEntities",
                column: "AuthorEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_ParentCommentEntityId",
                table: "CommentEntities",
                column: "ParentCommentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentEntities_PostEntityId",
                table: "CommentEntities",
                column: "PostEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_CommentEntityId",
                table: "CommentVoteEntities",
                column: "CommentEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityId",
                table: "CommentVoteEntities",
                column: "VoterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityId_VoterEntityUsername",
                table: "CommentVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_CommentVoteEntities_VoterEntityUsername",
                table: "CommentVoteEntities",
                column: "VoterEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityId",
                table: "CommunityEntities",
                column: "CreatorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityId_CreatorEntityUsername",
                table: "CommunityEntities",
                columns: new[] { "CreatorEntityId", "CreatorEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_CreatorEntityUsername",
                table: "CommunityEntities",
                column: "CreatorEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityEntities_Name",
                table: "CommunityEntities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityId_CommunityEnti~",
                table: "CommunityMembershipEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_CommunityEntityName",
                table: "CommunityMembershipEntities",
                column: "CommunityEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId",
                table: "CommunityMembershipEntities",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityId_UserEntityUsername",
                table: "CommunityMembershipEntities",
                columns: new[] { "UserEntityId", "UserEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembershipEntities_UserEntityUsername",
                table: "CommunityMembershipEntities",
                column: "UserEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigsEntities_EnvironmentName",
                table: "ConfigsEntities",
                column: "EnvironmentName",
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
                name: "IX_GoogleUserEntities_GmailAddress",
                table: "GoogleUserEntities",
                column: "GmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUserEntities_UserEntityId",
                table: "GoogleUserEntities",
                column: "UserEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityId",
                table: "PostEntities",
                column: "AuthorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityId_AuthorEntityUsername",
                table: "PostEntities",
                columns: new[] { "AuthorEntityId", "AuthorEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_AuthorEntityUsername",
                table: "PostEntities",
                column: "AuthorEntityUsername");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityId",
                table: "PostEntities",
                column: "CommunityEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityId_CommunityEntityName",
                table: "PostEntities",
                columns: new[] { "CommunityEntityId", "CommunityEntityName" });

            migrationBuilder.CreateIndex(
                name: "IX_PostEntities_CommunityEntityName",
                table: "PostEntities",
                column: "CommunityEntityName");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_PostEntityId",
                table: "PostVoteEntities",
                column: "PostEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityId",
                table: "PostVoteEntities",
                column: "VoterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityId_VoterEntityUsername",
                table: "PostVoteEntities",
                columns: new[] { "VoterEntityId", "VoterEntityUsername" });

            migrationBuilder.CreateIndex(
                name: "IX_PostVoteEntities_VoterEntityUsername",
                table: "PostVoteEntities",
                column: "VoterEntityUsername");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessTokenEntities");

            migrationBuilder.DropTable(
                name: "CommentVoteEntities");

            migrationBuilder.DropTable(
                name: "CommunityMembershipEntities");

            migrationBuilder.DropTable(
                name: "ConfigsEntities");

            migrationBuilder.DropTable(
                name: "EmailEntities");

            migrationBuilder.DropTable(
                name: "ErrorEntities");

            migrationBuilder.DropTable(
                name: "FileEntities");

            migrationBuilder.DropTable(
                name: "GoogleUserEntities");

            migrationBuilder.DropTable(
                name: "PostVoteEntities");

            migrationBuilder.DropTable(
                name: "RequestTraceEntities");

            migrationBuilder.DropTable(
                name: "SessionEntities");

            migrationBuilder.DropTable(
                name: "CommentEntities");

            migrationBuilder.DropTable(
                name: "PostEntities");

            migrationBuilder.DropTable(
                name: "CommunityEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
