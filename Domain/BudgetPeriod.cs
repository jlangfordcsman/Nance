using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class BudgetPeriod
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }


        public bool Contains(DateTime date)
        {
            return Start <= date && date <= End;
        }
    }
}
