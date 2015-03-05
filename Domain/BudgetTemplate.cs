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
                    itemTemplate
                        .Create(budget.Period)
                        .ToList()
                        .ForEach(item =>
                            {
                                budget
                                .BudgetedItems
                                .Add(item);
                            });
                }
            }

            return budget;
        }

        private BudgetPeriod GetTwiceAMonthPeriod(DateTime date)
        {

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


    }
}
