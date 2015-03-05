using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class RecursEveryPeriodRule : RecurringRule
    {
        public int? Day { get; set; }

        protected override bool CheckIfInPeriod(BudgetPeriod period)
        {
            return OccursWhileRuleIsEffective(period);
        }

        protected override ICollection<DateTime> DoGetDatesInPeriod(DateTime start, DateTime end)
        {
            var dueDates = new List<DateTime>();

            if (Day != null)
            {
                switch (Day)
                {
                    case (int)SpecialDays.First:
                        dueDates.Add(start);
                        break;

                    case (int)SpecialDays.Last:
                        dueDates.Add(end);
                        break;

                    default:
                        if (start.AddDays(Day.Value) <= end)
                        {
                            dueDates.Add(start.AddDays(Day.Value));
                        }
                        break;
                }
            }

            return dueDates;
        }
    }
}
