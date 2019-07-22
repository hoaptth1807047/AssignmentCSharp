namespace SunBank.entity
{
    public class TransactionSB
    {

        public enum TransactionType
        {
            Withdraw = 1,
            Deposit = 2, 
            Transfer = 3
        }
        
        public enum TransactionStatus
        {
            Completed = 1,
            Pending = 0,
            Deleted = -1
        }
        
        public string TransactionId { get; set; }
        public string SenderAccountNumber { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public TransactionType Type{ get; set; }
        public double Amount { get; set; }
        public string Message { get; set; }
        public long CreatedAtMls { get; set; }
        public long UpdatedAtMls { get; set; }
        public TransactionStatus Status { get; set; }
        
        public TransactionSB()
        {
        }

        public TransactionSB(string transactionId, string senderAccountNumber, string receiverAccountNumber, TransactionType type, double amount, string message, long createdAtMls, long updatedAtMls, TransactionStatus status)
        {
            TransactionId = transactionId;
            SenderAccountNumber = senderAccountNumber;
            ReceiverAccountNumber = receiverAccountNumber;
            Type = type;
            Amount = amount;
            Message = message;
            CreatedAtMls = createdAtMls;
            UpdatedAtMls = updatedAtMls;
            Status = status;
        }
    }
}