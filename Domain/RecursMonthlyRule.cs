using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class RecursMonthlyRule : RecurringRule
    {
        const int MinDay = 2;
        const int MaxDay = 28;

        private int day;

        public int Day 
        {
            get
            {
                return this.day;
            }

            set
            {
                if (value < RecursMonthlyRule.MinDay)
                {
                    this.day = (int)SpecialDays.First;
                }
                else if (value > RecursMonthlyRule.MaxDay)
                {
                    this.day = (int)SpecialDays.Last;
                }
                else
                {
                    this.day = value;
                }
            }
        }

        protected override ICollection<DateTime> DoGetDatesInPeriod(DateTime start, DateTime end)
        {
            var dueDates = new List<DateTime>();

            for (DateTime curDate = start; curDate < end; curDate = curDate.AddDays(1))
            {
                switch (day)
                {
                    case (int)SpecialDays.First:
                        if (curDate.Day == 1)
                        {
                            dueDates.Add(curDate);
                            break;
                        }
                        break;

                    case (int)SpecialDays.Last:
                        if (DateTime.DaysInMonth(curDate.Year, curDate.Month) == curDate.Day)
                        {
                            dueDates.Add(curDate);
                            break;
                        }
                        break;

                    default:
                        if (curDate.Day == day)
                        {
                            dueDates.Add(curDate);
                            break;
                        }

                        break;
                }
            }

            return dueDates;
        }

        protected override ICollection<BudgetPeriod> DoGetPeriodsInDateRange(DateTime start, DateTime end)
        {
            BudgetPeriod period = new BudgetPeriod();

            for (DateTime date = start; date.Month <= end.Month; date = date.AddMonths(1))
            {
                int startDayOfThisMonth = GetDay(date, FirstPayDay.Value);

                DateTime thisMonthsStartDate = new DateTime(date.Year, date.Month, startDayOfThisMonth);

                if (date <= thisMonthsStartDate)
                {
                    DateTime prevMonth = date.AddMonths(-1);
                    int startDayOfLastMonth = GetDay(prevMonth, FirstPayDay.Value);
                    period.Start = new DateTime(prevMonth.Year, prevMonth.Month, startDayOfLastMonth);

                    period.End = thisMonthsStartDate;
                }
                else
                {
                    DateTime nextMonth = date.AddMonths(1);
                    int startDayOfNextMonth = GetDay(nextMonth, FirstPayDay.Value);

                    period.Start = thisMonthsStartDate;
                    period.End = new DateTime(nextMonth.Year, nextMonth.Month, startDayOfNextMonth);
                }
            }

            return period;
        }

        protected override bool CheckIfInPeriod(BudgetPeriod period)
        {
            bool effective = OccursWhileRuleIsEffective(period);
            bool dayIsInPeriod = false;
            
            if (effective)
            {
                for (DateTime curDate = period.Start; curDate < period.End; curDate = curDate.AddDays(1))
                {
                    if (this.Day == (int)SpecialDays.Last && DateTime.DaysInMonth(curDate.Year, curDate.Month) == curDate.Day)
                    {
                        dayIsInPeriod = true;
                        break;
                    }

                    if (this.Day == (int)SpecialDays.First && curDate.Day == 1)
                    {
                        dayIsInPeriod = true;
                        break;
                    }

                    if (this.Day == curDate.Day)
                    {
                        dayIsInPeriod = true;
                        break;
                    }
                }
            }

            return effective && dayIsInPeriod;
        }
    }
}
