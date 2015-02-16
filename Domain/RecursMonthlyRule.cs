using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class RecursMonthlyRule : RecurringItemRule
    {
        const int MinDay = 2;
        const int MaxDay = 28;

        private int day;

        //HACK: 
        public enum SpecialDays { First=0, Last=99};

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
