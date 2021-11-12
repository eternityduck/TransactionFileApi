namespace Domain
{
    public enum Status
    {
        Pending,
        Cancelled,
        Completed
    }

    public enum TransactionType
    {
        Withdrawal,
        Refill
    }
    public class Transaction
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Client { get; set; }
    }
}