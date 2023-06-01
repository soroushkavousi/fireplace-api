using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ManualUpdateUsername : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var sql = @"
                CREATE OR REPLACE PROCEDURE public.""UpdateUsername""(
	                ""UserId"" numeric,
	                ""NewUsername"" text)
                LANGUAGE 'sql'
                AS $BODY$
                UPDATE public.""UserEntities""
                SET ""Username"" = ""NewUsername""
                WHERE ""Id"" = ""UserId"";
                $BODY$;";

        migrationBuilder.Sql(sql);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        var sql = @"
                DROP PROCEDURE public.""UpdateUsername"";
                ";

        migrationBuilder.Sql(sql);
    }
}
