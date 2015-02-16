using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class NanceUser
    {
        private BudgetTemplate currentBudgetTemplate;

        public NanceUser()
        {
            Budgets = new List<Budget>();
        }

        public Guid? Id { get; set; }

        public string DisplayName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string MMSNumber { get; set; }

        public Budget CurrentBudget 
        {
            get
            {

                if (!this.Budgets.Any(b => b.Period.Contains(DateTime.Now)))
                {
                    this.Budgets.Add(CurrentBudgetTemplate.Create(DateTime.Now));
                }

                return Budgets.FirstOrDefault(b => b.Period.Contains(DateTime.Now));
            }
        }

        public ICollection<Budget> Budgets { get; set; }

        public ICollection<BudgetTemplate> BudgetTemplates { get; set; }

        public BudgetTemplate CurrentBudgetTemplate 
        {
            get
            {
                if (currentBudgetTemplate == null)
                {
                    if (!BudgetTemplates.Any())
                    {
                        BudgetTemplates.Add(BudgetTemplate.Default);
                    }

                    currentBudgetTemplate = BudgetTemplates.Last();
                }


                return currentBudgetTemplate;
            }

            set
            {
                currentBudgetTemplate = value;
            }
        }
        
    }
}
