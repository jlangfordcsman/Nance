using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class BudgetTemplate
    {
        private static BudgetTemplate defaultTemplate;

        const int MinDay = 2;
        const int MaxDay = 28;

        public enum ValidPayIntervals { OnceAWeek, OnceEveryTwoWeeks, OnceAMonth, TwiceAMonth }
        public enum SpecialDays { First = 1, Last = 99 };

        public BudgetTemplate()
        {
            BudgetItemTemplates = new List<BudgetItemTemplate>();
        }

        public Guid? Id { get; set; }

        DayOfWeek? FirstDayOfWeekPaid { get; set; }
        DayOfWeek? SecondDayOfWeekPaid { get; set; }

        DateTime? InitialPayDate { get; set; }

        public static BudgetTemplate Default
        {
            get
            {
                if (BudgetTemplate.defaultTemplate == null)
                {
                    defaultTemplate =
                        new BudgetTemplate()
                        {
                            EmployerName = "New employer",
                            PayInterval = ValidPayIntervals.OnceAMonth,
                            FirstPayDay = 1,
                            SecondPayDay = null
                        };
                }

                return defaultTemplate;
            }
        }

        public ValidPayIntervals PayInterval { get; set; }

        public int? FirstPayDay { get; set; }
        public int? SecondPayDay { get; set; }

        string EmployerName { get; set; }

        public ICollection<BudgetItemTemplate> BudgetItemTemplates { get; set; }

        public Budget Create(DateTime date)
        {
            BudgetPeriod period;

            switch (this.PayInterval)
            {
                case ValidPayIntervals.OnceAMonth:
                    period = GetOnceAMonthPeriod(date);
                    break;

                case ValidPayIntervals.OnceAWeek:
                    period = GetOnceAWeekPeriod(date);
                    break;

                case ValidPayIntervals.OnceEveryTwoWeeks:
                    period = GetOnceEveryTwoWeeksPeriod(date);
                    break;

                case ValidPayIntervals.TwiceAMonth:
                    period = GetTwiceAMonthPeriod(date);
                    break;

                default:
                    throw new NotImplementedException("This budget type has not been implemented yet");
            }

            Budget budget = new Budget() { Period = period };

            foreach (var itemTemplate in this.BudgetItemTemplates)
            {
                if (itemTemplate.InPeriod(budget.Period))
                {
                    budget
                        .BudgetedItems
                        .Add(itemTemplate.Create(budget.Period));
                }
            }

            return budget;
        }

        private BudgetPeriod GetTwiceAMonthPeriod(DateTime date)
        {
            BudgetPeriod period;
            period = new BudgetPeriod();

            int firstDay = FirstPayDay == null ? 1 : FirstPayDay.Value;
            int secondDay = SecondPayDay == null ? 15 : SecondPayDay.Value;

            if (secondDay < firstDay)
            {
                secondDay = firstDay + 7;
            }

            if (secondDay > 28)
            {
                secondDay = (int)SpecialDays.Last;
            }


            DateTime prevMonth = date.AddMonths(-1);
            DateTime nextMonth = date.AddMonths(1);

            DateTime secondDayLastMonth = new DateTime(prevMonth.Year, prevMonth.Month, GetDay(prevMonth, secondDay));
            DateTime firstDayThisMonth = new DateTime(date.Year, date.Month, GetDay(date, firstDay));
            DateTime secondDayThisMonth = new DateTime(date.Year, date.Month, GetDay(date, secondDay));
            DateTime firstDayNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, GetDay(nextMonth, firstDay));

            if (secondDayLastMonth < date && date < firstDayThisMonth)
            {
                period.Start = secondDayLastMonth;
                period.End = firstDayThisMonth;
            }
            else if (firstDayThisMonth < date && date < secondDayThisMonth)
            {
                period.Start = firstDayThisMonth;
                period.End = secondDayThisMonth;
            }
            else
            {
                period.Start = secondDayThisMonth;
                period.End = firstDayNextMonth;
            }
            return period;
        }

        private BudgetPeriod GetOnceEveryTwoWeeksPeriod(DateTime date)
        {
            BudgetPeriod period;
            period = new BudgetPeriod();

            DateTime initDate = InitialPayDate == null ? DateTime.MinValue : InitialPayDate.Value;
            var periodsElapsed = (int)((date - initDate).Days / 14);

            period.Start = initDate.AddDays(periodsElapsed * 14);
            period.End = period.Start.AddDays(14);
            return period;
        }

        private BudgetPeriod GetOnceAWeekPeriod(DateTime date)
        {
            BudgetPeriod period;
            period = new BudgetPeriod();
            DayOfWeek payDay = FirstDayOfWeekPaid ?? DayOfWeek.Friday;

            DateTime startDate;

            for (startDate = date; startDate.DayOfWeek != payDay; startDate = startDate.AddDays(-1)) ;

            period.Start = startDate;
            period.End = startDate.AddDays(7);
            return period;
        }

        private BudgetPeriod GetOnceAMonthPeriod(DateTime date)
        {
            BudgetPeriod period = new BudgetPeriod();

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

            return period;
        }

        private static int GetDay(DateTime date, int day)
        {
            if (day < BudgetTemplate.MinDay && day != (int)SpecialDays.First)
            {
                throw new InvalidOperationException("The day is less than the minimum allowed day");
            }

            if (day > BudgetTemplate.MaxDay && day != (int)SpecialDays.Last)
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

    }
}
