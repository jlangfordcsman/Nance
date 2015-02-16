using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class Budget
    {
        public Budget()
        {
            Id = Guid.NewGuid();
            BudgetedItems = new List<BudgetItem>();
            PersonalDebits = new List<BudgetDebit>();
        }

        public Guid? Id { get; set; }

        public BudgetPeriod Period {get; set;}

        public ICollection<BudgetItem> BudgetedItems { get; set; }

        public ICollection<BudgetDebit> PersonalDebits { get; set; }

        public Decimal TotalDollarsRemaining
        {
            get
            {
                return PersonalDebits.Sum(d => d.Amount) - BudgetedItems.Sum(b => b.Paid);
            }
        }

        public Decimal TotalOutstanding
        {
            get
            {
                //throw new NotImplementedException();
                return 0.00m;
            }
        }
    }
}
