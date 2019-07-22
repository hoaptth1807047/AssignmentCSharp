namespace SunBank.entity
{
    public class TransactionBlockChain
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
        
        public TransactionBlockChain()
        {
        }

        public TransactionBlockChain(string transactionId, string senderAddress, string receiveAddress, TransactionType type, double amount, long createdAtMls, long updatedAtMls, TransactionStatus status)
        {
            TransactionId = transactionId;
            SenderAddress = senderAddress;
            ReceiveAddress = receiveAddress;
            Type = type;
            Amount = amount;
            CreatedAtMls = createdAtMls;
            UpdatedAtMls = updatedAtMls;
            Status = status;
        }

        public string TransactionId { get; set; }
        public string SenderAddress { get; set; }
        public string ReceiveAddress { get; set; }
        public TransactionType Type { get; set; }

        public double Amount { get; set; }
        
        public long CreatedAtMls { get; set; }
        
        public long UpdatedAtMls { get; set; }
        
        public TransactionStatus Status { get; set; }
    }
    
}