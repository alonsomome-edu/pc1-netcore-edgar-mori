using Banking.Net.Command.Transactions.Domain.Entities;
using FluentNHibernate.Mapping;

namespace Banking.Transactions.Command.Infra.Persistence.NHibernate.Mappings
{
    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            CompositeId(x => x.TransactionId).KeyProperty(x => x.Id, y => y.ColumnName("transaction_id"));
            Component(x => x.FromBankAccountId, m =>
            {
                m.Map(x => x.Id, "bank_account_from_id");
            });
            Component(x => x.ToBankAccountId, m =>
            {
                m.Map(x => x.Id, "bank_account_to_id");
            });
            Component(x => x.Amount, m =>
            {
                m.Map(x => x.Amount, "amount");
                m.Map(x => x.Currency, "currency");
            });
            Map(x => x.TransactionStateId).CustomType<int>().Column("transaction_state_id");
            Map(x => x.CreatedAt).Column("created_at_utc");
            Map(x => x.UpdatedAt).Column("updated_at_utc");
        }
    }
}