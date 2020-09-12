using System.Text;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Banking.Net.Common.Infra.Persistence.NHibernate
{
    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(GetTableName(instance.EntityType.Name));
        }

        private string GetTableName(string value)
        {
            StringBuilder builder = new StringBuilder();
            foreach (char c in value)
            {
                if (char.IsUpper(c) && builder.Length > 0)
                {
                    builder.Append('_');
                }
                builder.Append(c);
            }
            return builder.ToString().ToLower();
        }
    }
}
