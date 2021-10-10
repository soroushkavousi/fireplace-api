using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddCreationAndModifiedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "UserEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "UserEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "SessionEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "SessionEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "GoogleUserEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "GoogleUserEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "GlobalEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "GlobalEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "FileEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "FileEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "ErrorEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "ErrorEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "EmailEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "EmailEntities",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "AccessTokenEntities",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "AccessTokenEntities",
                type: "timestamp without time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "UserEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "UserEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "SessionEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "SessionEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "GoogleUserEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "GoogleUserEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "GlobalEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "GlobalEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "FileEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "FileEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "ErrorEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "ErrorEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "EmailEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "EmailEntities");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AccessTokenEntities");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "AccessTokenEntities");
        }
    }
}
