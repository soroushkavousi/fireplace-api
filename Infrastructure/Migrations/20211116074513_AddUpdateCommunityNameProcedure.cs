using Microsoft.EntityFrameworkCore.Migrations;

namespace FireplaceApi.Infrastructure.Migrations
{
    public partial class AddUpdateCommunityNameProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp =
@"CREATE OR REPLACE PROCEDURE public.""UpdateCommunityName""(
    ""CommunityId"" numeric,
    ""NewCommunityName"" text)
LANGUAGE 'sql'
AS $BODY$
    UPDATE public.""CommunityEntities""
    SET ""Name"" = ""NewCommunityName""
    WHERE ""Id"" = ""CommunityId"";
$BODY$;";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
