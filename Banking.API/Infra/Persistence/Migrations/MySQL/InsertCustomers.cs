using FluentMigrator;

namespace Banking.API.Infra.Persistence.Migrations.MySQL
{
    [Migration(2)]
    public class InsertCustomers : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("InsertCustomers.sql");
        }

        public override void Down()
        {
        }
    }
}