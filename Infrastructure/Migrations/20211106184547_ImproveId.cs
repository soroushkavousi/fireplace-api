using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System.Collections.Generic;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class ImproveId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "UserEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "UserEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "SessionEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "SessionEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostVoteEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "PostVoteEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostQueryResultEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "PostQueryResultEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "PostEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "GoogleUserEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "GoogleUserEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "GlobalEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "FileEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "FileEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ErrorEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "ErrorEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EmailEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "EmailEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");


            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityQueryResultEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommunityQueryResultEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityMembershipQueryResultEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommunityMembershipQueryResultEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityMembershipEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommunityMembershipEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommunityEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentVoteEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommentVoteEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentQueryResultEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommentQueryResultEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "CommentEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AccessTokenEntities")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "Id",
                table: "AccessTokenEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "UserEntityId",
                table: "SessionEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "VoterEntityId",
                table: "PostVoteEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "PostEntityId",
                table: "PostVoteEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<List<ulong>>(
                name: "ReferenceEntityIds",
                table: "PostQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(List<long>),
                oldType: "bigint[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "CommunityEntityId",
                table: "PostEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "AuthorEntityId",
                table: "PostEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "UserEntityId",
                table: "GoogleUserEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "UserEntityId",
                table: "EmailEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<List<ulong>>(
                name: "ReferenceEntityIds",
                table: "CommunityQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(List<long>),
                oldType: "bigint[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<ulong>>(
                name: "ReferenceEntityIds",
                table: "CommunityMembershipQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(List<long>),
                oldType: "bigint[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "UserEntityId",
                table: "CommunityMembershipEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "CommunityEntityId",
                table: "CommunityMembershipEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "CreatorEntityId",
                table: "CommunityEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "VoterEntityId",
                table: "CommentVoteEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "CommentEntityId",
                table: "CommentVoteEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<List<ulong>>(
                name: "ReferenceEntityIds",
                table: "CommentQueryResultEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(List<long>),
                oldType: "bigint[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "PostEntityId",
                table: "CommentEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<List<ulong>>(
                name: "ParentCommentEntityIds",
                table: "CommentEntities",
                type: "numeric(20,0)[]",
                nullable: true,
                oldClrType: typeof(List<long>),
                oldType: "bigint[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "AuthorEntityId",
                table: "CommentEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<ulong>(
                name: "UserEntityId",
                table: "AccessTokenEntities",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "UserEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "UserEntityId",
                table: "SessionEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "SessionEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "VoterEntityId",
                table: "PostVoteEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "PostEntityId",
                table: "PostVoteEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostVoteEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<List<long>>(
                name: "ReferenceEntityIds",
                table: "PostQueryResultEntities",
                type: "bigint[]",
                nullable: true,
                oldClrType: typeof(List<ulong>),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostQueryResultEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "CommunityEntityId",
                table: "PostEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "AuthorEntityId",
                table: "PostEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PostEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "UserEntityId",
                table: "GoogleUserEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "GoogleUserEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GlobalEntities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "FileEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ErrorEntities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "UserEntityId",
                table: "EmailEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "EmailEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<List<long>>(
                name: "ReferenceEntityIds",
                table: "CommunityQueryResultEntities",
                type: "bigint[]",
                nullable: true,
                oldClrType: typeof(List<ulong>),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityQueryResultEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<List<long>>(
                name: "ReferenceEntityIds",
                table: "CommunityMembershipQueryResultEntities",
                type: "bigint[]",
                nullable: true,
                oldClrType: typeof(List<ulong>),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityMembershipQueryResultEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "UserEntityId",
                table: "CommunityMembershipEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "CommunityEntityId",
                table: "CommunityMembershipEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityMembershipEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "CreatorEntityId",
                table: "CommunityEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommunityEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "VoterEntityId",
                table: "CommentVoteEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "CommentEntityId",
                table: "CommentVoteEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentVoteEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<List<long>>(
                name: "ReferenceEntityIds",
                table: "CommentQueryResultEntities",
                type: "bigint[]",
                nullable: true,
                oldClrType: typeof(List<ulong>),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentQueryResultEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "PostEntityId",
                table: "CommentEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<List<long>>(
                name: "ParentCommentEntityIds",
                table: "CommentEntities",
                type: "bigint[]",
                nullable: true,
                oldClrType: typeof(List<ulong>),
                oldType: "numeric(20,0)[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AuthorEntityId",
                table: "CommentEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "CommentEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "UserEntityId",
                table: "AccessTokenEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AccessTokenEntities",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
