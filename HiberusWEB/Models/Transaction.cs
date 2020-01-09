using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiberusWEB.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
