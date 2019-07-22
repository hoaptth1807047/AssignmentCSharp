namespace SunBank.entity
{
    public class SBAccount
    {
        public SBAccount()
        {
        }

        public SBAccount(string accountNumber, string userName, string password, double balance, int status)
        {
            AccountNumber = accountNumber;
            UserName = userName;
            Password = password;
            Balance = balance;
            Status = status;
        }

        public string AccountNumber { get ; set; }
        
        public string UserName { get; set; }

        public string Password { get ; set; }

        public double Balance { get ; set; }

        public int Status { get ; set; }
        
        public override string ToString()
        {
            return $"";
        }
    }
}