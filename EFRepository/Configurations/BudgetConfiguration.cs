using Nance.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.EFRepository.Configurations
{
    public class BudgetConfiguration : EntityTypeConfiguration<Budget>
    {
        public BudgetConfiguration()
        {
            Property(p => p.Period.End).HasColumnName("PeriodEnd");
            Property(p => p.Period.Start).HasColumnName("PeriodStart");
        }
    }
}
