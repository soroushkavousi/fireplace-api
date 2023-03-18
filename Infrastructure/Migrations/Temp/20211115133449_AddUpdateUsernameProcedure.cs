using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddUpdateUsernameProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE OR REPLACE PROCEDURE public.""UpdateUsername""(
	            ""UserId"" numeric,
	            ""NewUsername"" text)
            LANGUAGE 'sql'
            AS $BODY$
            UPDATE public.""UserEntities""
            SET ""Username"" = ""NewUsername""
            WHERE ""Id"" = ""UserId"";
            $BODY$;";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
