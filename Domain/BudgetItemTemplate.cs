using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class BudgetItemTemplate
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public RecurringRule RecurrenceRule { get; set; }

        public decimal Due { get; set; }

        public bool InPeriod(BudgetPeriod period)
        {
            return GetDueDates(period) != null;
        }

        public ICollection<BudgetItem> Create(BudgetPeriod period)
        {
            return GetDueDates(period)
                .Select(dueDate =>
            {
                return new BudgetItem()
                {
                    Description = this.Description,
                    Amount = this.Due,
                    DueDate = dueDate
                };
            }).ToList();
        }

        private ICollection<DateTime> GetDueDates(BudgetPeriod period)
        {
            return this.RecurrenceRule == null ? null : this.RecurrenceRule.GetDatesInPeriod(period);
        }

    }
}
