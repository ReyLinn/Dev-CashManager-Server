using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashManager.Models
{
    public class PaymentConfig
    {
        public int NbOfWrongCards { get; set; }
        public int NbOfWrongCheques { get; set; }
        public float MaximumCostOfTransaction { get; set; }
        public int NumberOfTransactionPerMinute { get; set; }
    }
}
