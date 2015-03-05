using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public abstract class RecurringRule
    {
        protected const int MinAllowedDayOfMonth = 1;
        protected const int MaxAllowedDayOfMonth = 28;

        public enum SpecialDays { First = 99, Last = 99}

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

        public ICollection<DateTime> GetDatesInPeriod(BudgetPeriod period)
        {
            return DoGetDatesInPeriod(period.Start, period.End);
        }

        public ICollection<BudgetPeriod> GetPeriodsInDateRange(DateTime startDate, DateTime endDate)
        {
            return DoGetPeriodsInDateRange(startDate, endDate);
        }

        protected int GetDay(DateTime date, int day)
        {
            if (day < RecurringRule.MinAllowedDayOfMonth && day != (int)SpecialDays.First)
            {
                throw new InvalidOperationException("The day is less than the minimum allowed day");
            }

            if (day > RecurringRule.MaxAllowedDayOfMonth && day != (int)SpecialDays.Last)
            {
                throw new InvalidOperationException("The day is greater than the maximum allowed day or SpecialDays.Last");
            }

            int actualDay = day;

            if (day == (int)SpecialDays.First)
            {
                actualDay = 1;
            }
            else if (day == (int)SpecialDays.Last)
            {
                actualDay = DateTime.DaysInMonth(date.Year, date.Month);
            }

            return actualDay;
        }

        protected abstract ICollection<BudgetPeriod> DoGetPeriodsInDateRange(DateTime start, DateTime end);

        protected abstract ICollection<DateTime> DoGetDatesInPeriod(DateTime startDate, DateTime endDate);

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
