using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class RecursEveryPeriodRule : RecurringItemRule
    {
        protected override bool CheckIfInPeriod(BudgetPeriod period)
        {
            return OccursWhileRuleIsEffective(period);
        }
    }
}
