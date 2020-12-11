namespace CashManager.Models
{
    /// <summary>
    /// The payment Configuration from a Json File
    /// </summary>
    public class PaymentConfig
    {
        public int NbOfWrongCards { get; set; }
        public int NbOfWrongCheques { get; set; }
        public float MaximumCostOfTransaction { get; set; }
        public int NumberOfTransactionPerMinute { get; set; }
    }
}
