using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public abstract class RecurringItemRule
    {
        public string Name { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? RetireDate { get; set; }

        public ICollection<RuleAction> Actions { get; set; }

        public void PerformActions(Budget budget)
        {
            foreach (var action in Actions)
            {
                action.Perform(budget);
            }
        }

        public bool OccursInPeriod(BudgetPeriod period)
        {
            return CheckIfInPeriod(period);
        }


        protected bool OccursWhileRuleIsEffective(BudgetPeriod period)
        {
            bool startsAfterRuleIsEffective = true;
            bool endsBeforeRuleIsRetired = true;

            if (this.EffectiveDate != null)
            {
                startsAfterRuleIsEffective = period.Start >= this.EffectiveDate;
            }

            if (this.RetireDate != null)
            {
                endsBeforeRuleIsRetired = period.End <= this.RetireDate;
            }

            return startsAfterRuleIsEffective && endsBeforeRuleIsRetired;
        }

        protected abstract bool CheckIfInPeriod(BudgetPeriod period);
    }
}
