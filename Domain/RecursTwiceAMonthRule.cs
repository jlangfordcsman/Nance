using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class RecursTwiceAMonthRule : RecurringRule
    {
        public int FirstPayDay { get; set; }
        public int SecondPayDay { get; set; }

        protected override ICollection<DateTime> DoGetDatesInPeriod(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        protected override ICollection<BudgetPeriod> DoGetPeriodsInDateRange(DateTime start, DateTime end)
        {
            BudgetPeriod period;
            period = new BudgetPeriod();

            int firstDay = FirstPayDay;
            int secondDay = SecondPayDay;

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
    }
}
