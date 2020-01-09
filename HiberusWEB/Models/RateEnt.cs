using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusWEB.Models
{
    public class RateEnt
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal Rate { get; set; }
    }
}
