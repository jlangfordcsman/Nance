using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class BudgetItem
    {
        public BudgetItem()
        {
            Amount = 0.00m;
            Payments = new List<BudgetItemPayment>();
        }

        public Guid? Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    
        public decimal Paid 
        {
            get
            {
                var totalPaid = Payments.Sum(p => p.Amount);
                return totalPaid ?? 0.00m;
            }
        }

        public decimal Remaining
        {
            get
            {
                var remaining = Amount - Paid;
                if (remaining < 0.00m)
                {
                    remaining = 0.00m;
                }

                return remaining;
            }
        }

        public decimal OverPaid
        {
            get
            {
                var overpaid = Amount - Paid;

                if (overpaid > 0.00m)
                {
                    overpaid = 0.00m;
                }

                return Math.Abs(overpaid);
            }
        }

        public DateTime? DueDate { get; set; }

        public DateTime? PaidDate { get; set; }

        public string Notes { get; set; }

        public ICollection<BudgetItemPayment> Payments { get; set; }
    }
}
