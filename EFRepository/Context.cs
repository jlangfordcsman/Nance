using Nance.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.EFRepository
{
    public class Context : DbContext
    {
        public Context()
            : base(@"Data Source=Tertius;Initial Catalog=Nance; Integrated Security=True; MultipleActiveResultSets=True")
        {
        }

        public DbSet<NanceUser> Users { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetItem> BudgetItems { get; set; }
        public DbSet<BudgetItemPayment> BudgetItemPayments { get; set; }
        public DbSet<BudgetItemTemplate> BudgetItemTemplates { get; set; }
        public DbSet<BudgetTemplate> BudgetTemplates { get; set; }
        public DbSet<BudgetDebit> BudgetDebits { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Configurations.BudgetConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
