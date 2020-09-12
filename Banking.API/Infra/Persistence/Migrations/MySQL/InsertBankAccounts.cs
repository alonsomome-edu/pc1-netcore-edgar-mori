using FluentMigrator;

namespace Banking.API.Infra.Persistence.Migrations.MySQL
{
    [Migration(3)]
    public class InsertBankAccounts : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("InsertBankAccounts.sql");
        }

        public override void Down()
        {
        }
    }
}
