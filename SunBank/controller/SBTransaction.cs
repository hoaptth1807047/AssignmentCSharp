using System;
using SunBank.entity;
using SunBank.model;

namespace SunBank
{
    public class SBTransaction : Transaction
    {

        private static SBAccountModel sbAccountModel;


        public SBTransaction()
        {
            sbAccountModel = new SBAccountModel();
        }
        
        
        public void Login()
        {
            Console.WriteLine("Please enter your username: ");
            var userName = Console.ReadLine();
            Console.WriteLine("Please enter your password: ");
            var password = Console.ReadLine();
            var sbAccount = sbAccountModel.FindByUserNameAndPassword(userName, password);
            if (sbAccount == null)
            {
                Console.WriteLine("Wrong account information. Please login again!");
                Console.WriteLine("Press any key to continue!");
                Console.Read();
            }

            Program.currentLoggedInAccount = sbAccount;
        }
        public void Withdraw()
        {
            if (Program.currentLoggedInAccount != null)
            {
                Console.Clear();
                Console.WriteLine("Withdraw money at the SB system.");
                Console.WriteLine("Please enter the amount to withdraw: ");
                var amount = double.Parse(Console.ReadLine());
                if (amount <= 0)
                {
                    Console.WriteLine("Invalid amount. Please try again!");
                    return;
                }

                var transaction = new TransactionSB
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SenderAccountNumber = Program.currentLoggedInAccount.AccountNumber,
                    ReceiverAccountNumber = Program.currentLoggedInAccount.AccountNumber,
                    Type = TransactionSB.TransactionType.Withdraw,
                    Amount = amount,
                    Message = "Withdraw money in the ATM with amount: " + amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = (TransactionSB.TransactionStatus) 1
                };
                if (sbAccountModel.UpdateBalance(Program.currentLoggedInAccount, transaction))
                {
                    Console.Clear();
                    Console.WriteLine("Withdraw success!");
                    Console.WriteLine("Continue with SB-Bank? (y/n)");
                    if (Console.ReadLine().ToLower().Equals("n"))
                    {
                        Program.currentLoggedInAccount = null;
                    }
                }
                else
                {
                    Console.WriteLine("Transaction failed. Please try again later!\nEnter to continue!");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Please login to use this function!");
            }
        }

        public void Deposit()
        {
            if (Program.currentLoggedInAccount != null)
            {
                Console.Clear();
                Console.WriteLine("Deposit money at the SB system.");
                Console.WriteLine("Please enter the amount to deposit: ");
                var amount = double.Parse(Console.ReadLine());
            
                var transaction = new TransactionSB
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SenderAccountNumber = Program.currentLoggedInAccount.AccountNumber,
                    ReceiverAccountNumber = Program.currentLoggedInAccount.AccountNumber,
                    Type = TransactionSB.TransactionType.Deposit,
                    Amount = amount,
                    Message = "Deposit money in the ATM with amount: " + amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = (TransactionSB.TransactionStatus) 1
                };

                if (sbAccountModel.UpdateBalance(Program.currentLoggedInAccount, transaction))
                {
                    Console.Clear();
                    Console.WriteLine("Deposit success!");
                    Console.WriteLine("Continue with SB-Bank?(y/n)");
                    if (Console.ReadLine().ToLower().Equals("n"))
                    {
                        Program.currentLoggedInAccount = null;
                    }
                }
                else
                {
                    Console.WriteLine("Transaction failed. Please try again later!\nEnter to continue!");
                    Console.ReadLine();
                
                }
            }
            else
            {
                Console.WriteLine("Please login to use this function!");
            }
        }

        public void Transfer()
        {
            if (Program.currentLoggedInAccount != null)
            {
                Console.Clear();
                Console.WriteLine("Transfer money at the SB system.");
                Console.WriteLine("Please enter the amount to transfer: ");
                var amount = double.Parse(Console.ReadLine());
                Console.WriteLine("Enter ReceiverAccountNumber:");
                var receiverAccountNumber = Console.ReadLine();
            
                var transaction = new TransactionSB
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SenderAccountNumber = Program.currentLoggedInAccount.AccountNumber,
                    ReceiverAccountNumber = receiverAccountNumber,
                    Type = TransactionSB.TransactionType.Transfer,
                    Amount = amount,
                    Message = "Transfer money in the ATM with amount: " + amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = (TransactionSB.TransactionStatus) 1
                };

                if (sbAccountModel.Transfer(Program.currentLoggedInAccount, transaction))
                {
                    Console.WriteLine("Transfers success!");
                    Console.WriteLine("Continue with SHB-Bank? (y/n)");
                    if (Console.ReadLine().ToLower().Equals("n"))
                    {
                        Program.currentLoggedInAccount = null;
                    }
                }
                else
                {
                    Console.WriteLine("Transaction failed. Please try again later!\nEnter to continue!");
                    Console.ReadLine();
                
                }
            }
            else
            {
                Console.WriteLine("Please login to use this function!");
            }        }
    }
}