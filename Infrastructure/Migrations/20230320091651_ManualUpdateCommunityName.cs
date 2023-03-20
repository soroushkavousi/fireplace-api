using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireplaceApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManualUpdateCommunityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                CREATE OR REPLACE PROCEDURE public.""UpdateCommunityName""(
	                ""CommunityId"" numeric,
	                ""NewCommunityName"" text)
                LANGUAGE 'sql'
                AS $BODY$
                UPDATE public.""CommunityEntities""
                SET ""Name"" = ""NewCommunityName""
                WHERE ""Id"" = ""CommunityId"";
                $BODY$;";

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                DROP PROCEDURE public.""UpdateCommunityName"";
                ";

            migrationBuilder.Sql(sql);
        }
    }
}
