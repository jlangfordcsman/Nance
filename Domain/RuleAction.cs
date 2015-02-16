using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nance.Domain
{
    public abstract class RuleAction
    {
        public abstract string Name { get; }

        public DateTime? DueDate { get; set; }

        public abstract void Perform(Budget budget);
    }
}
