using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class BudgetItemTemplate
    {
        public enum SpecialDays { First, Last }

        public enum ValidInterval { Monthly, EveryPeriod }

        public Guid? Id { get; set; }

        public int? Day { get; set; }

        public DayOfWeek? DayOfWeek { get; set; }

        public ValidInterval Interval { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Due { get; set; }

        public bool InPeriod(BudgetPeriod period)
        {
            return GetDueDate(period) != null;
        }

        public BudgetItem Create(BudgetPeriod period)
        {
            return new BudgetItem()
            {
                Description = this.Description,
                Amount = this.Due,
                DueDate = GetDueDate(period)
            };
        }

        private DateTime? GetDueDate(BudgetPeriod period)
        {
            DateTime? dueDate = null;

            switch (Interval)
            {
                case ValidInterval.EveryPeriod:
                    dueDate = GetEveryPeriodDueDate(period, dueDate);
                    break;

                case ValidInterval.Monthly:
                    int day = Day == null ? (int)SpecialDays.Last : Day.Value;

                    for (DateTime curDate = period.Start; curDate < period.End; curDate = curDate.AddDays(1))
                    {
                        switch (day)
                        {
                            case (int)SpecialDays.First:
                                if (curDate.Day == 1)
                                {
                                    dueDate = curDate;
                                    break;
                                }
                                break;

                            case (int)SpecialDays.Last:
                                if (DateTime.DaysInMonth(curDate.Year, curDate.Month) == curDate.Day)
                                {
                                    dueDate = curDate;
                                    break;
                                }
                                break;

                            default:
                                if (curDate.Day == day)
                                {
                                    dueDate = curDate;
                                    break;
                                }

                                break;
                        }
                    }
                    break;
            }

            return dueDate;
        }

        private DateTime? GetEveryPeriodDueDate(BudgetPeriod period, DateTime? dueDate)
        {
            if (Day != null)
            {
                switch (Day)
                {
                    case (int)SpecialDays.First:
                        dueDate = period.Start;
                        break;

                    case (int)SpecialDays.Last:
                        dueDate = period.End;
                        break;

                    default:
                        if (period.Start.AddDays(Day.Value) <= period.End)
                        {
                            dueDate = period.Start.AddDays(Day.Value);
                        }
                        break;
                }
            }
            return dueDate;
        }
    }
}
