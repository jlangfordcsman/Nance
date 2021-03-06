﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public class BudgetItemPayment
    {
        public Guid? Id { get; set; }

        public string Description { get; set; }

        public DateTime? PaidDate { get; set; }

        public decimal? Amount { get; set; }

        public string Notes { get; set; }
    }
}
