using FluentMigrator;

namespace Banking.API.Infra.Persistence.Migrations.MySQL
{
    [Migration(1)]
    public class CreateInitialschema : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("CreateInitialSchema.sql");
        }

        public override void Down()
        {
        }
    }
}