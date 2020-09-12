using Banking.Net.Command.Customers.Domain.Entities;
using FluentNHibernate.Mapping;

namespace Banking.Net.Command.Customers.Infra.Persistence.NHibernate.Mapping
{
    public class CustomerMap : ClassMap<Customer>
    {
        public CustomerMap()
        {
            CompositeId(x => x.CustomerId).KeyProperty(x => x.Id, y => y.ColumnName("customer_id"));
            Component(x => x.Name, m =>
            {
                m.Map(x => x.FirstName, "first_name");
                m.Map(x => x.LastName, "last_name");
            });
            Map(x => x.Active).CustomType<bool>().Column("is_active");
            Map(x => x.CreatedAt).Column("created_at_utc");
            Map(x => x.UpdatedAt).Column("updated_at_utc");
        }
    }
}