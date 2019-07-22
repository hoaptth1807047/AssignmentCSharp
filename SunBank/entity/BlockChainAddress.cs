namespace SunBank.entity
{
    public class BlockChainAddress
    {
        public BlockChainAddress()
        {
        }

        public BlockChainAddress(string address, string privateKey, double balance, long createdAtMls, long updatedAtMls)
        {
            Address = address;
            PrivateKey = privateKey;
            Balance = balance;
            CreatedAtMls = createdAtMls;
            UpdatedAtMls = updatedAtMls;
        }

        public string Address { get; set; }
        
        public string PrivateKey { get; set; }
        
        public double Balance { get; set; }
        public long CreatedAtMls{ get; set; }
        public long UpdatedAtMls{ get; set; }
    }
}