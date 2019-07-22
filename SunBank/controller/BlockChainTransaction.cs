using System;
using SunBank.entity;
using SunBank.model;

namespace SunBank
{
    public class BlockChainTransaction : Transaction
    {

        private static BlockChainAddressModel bcAddressModel;


        public BlockChainTransaction()
        {
            bcAddressModel = new BlockChainAddressModel();
        }
        
        
        public void Login()
        {
            Console.WriteLine("Please enter your address: ");
            var address = Console.ReadLine();
            Console.WriteLine("Please enter your privateKey: ");
            var privateKey = Console.ReadLine();
            var bcAddress = bcAddressModel.FindByAddressAndPrivateKey(address, privateKey);
            if ( bcAddress == null)
            {
                Console.WriteLine("Wrong account information. Please login again!");
                Console.WriteLine("Press any key to continue!");
                Console.Read();
            }

            Program.currentLoggedInAddress = bcAddress;
        }
        public void Withdraw()
        {
            if (Program.currentLoggedInAddress != null)
            {
                Console.Clear();
                Console.WriteLine("Withdraw money at the BlockChain system.");
                Console.WriteLine("Please enter the amount to withdraw: ");
                var amount = double.Parse(Console.ReadLine());
                if (amount <= 0)
                {
                    Console.WriteLine("Invalid amount. Please try again!");
                    return;
                }

                var transaction = new TransactionBlockChain
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SenderAddress = Program.currentLoggedInAddress.Address,
                    ReceiveAddress = Program.currentLoggedInAddress.Address,
                    Type = TransactionBlockChain.TransactionType.Withdraw,
                    Amount = amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = (TransactionBlockChain.TransactionStatus) 1
                };
                if (bcAddressModel.UpdateBalance(Program.currentLoggedInAddress, transaction))
                {
                    Console.Clear();
                    Console.WriteLine("Withdraw success!");
                    Console.WriteLine("Continue with BlockChain? (y/n)");
                    if (Console.ReadLine().ToLower().Equals("n"))
                    {
                        Program.currentLoggedInAddress = null;
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
            if (Program.currentLoggedInAddress != null)
            {
                Console.Clear();
                Console.WriteLine("Deposit money at the BlockChain system.");
                Console.WriteLine("Please enter the amount to deposit: ");
                var amount = double.Parse(Console.ReadLine());
            
                var transaction = new TransactionBlockChain()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SenderAddress = Program.currentLoggedInAddress.Address,
                    ReceiveAddress = Program.currentLoggedInAddress.Address,
                    Type = TransactionBlockChain.TransactionType.Deposit,
                    Amount = amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = (TransactionBlockChain.TransactionStatus) 1
                };

                if (bcAddressModel.UpdateBalance(Program.currentLoggedInAddress, transaction))
                {
                    Console.Clear();
                    Console.WriteLine("Deposit success!");
                    Console.WriteLine("Continue with BlockChain?(y/n)");
                    if (Console.ReadLine().ToLower().Equals("n"))
                    {
                        Program.currentLoggedInAddress = null;
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
                var receiverAddress = Console.ReadLine();
            
                var transaction = new TransactionBlockChain()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SenderAddress = Program.currentLoggedInAddress.Address,
                    ReceiveAddress = receiverAddress,
                    Type = TransactionBlockChain.TransactionType.Transfer,
                    Amount = amount,
                    CreatedAtMls = DateTime.Now.Ticks,
                    UpdatedAtMls = DateTime.Now.Ticks,
                    Status = (TransactionBlockChain.TransactionStatus) 1
                };

                if (bcAddressModel.Transfer(Program.currentLoggedInAddress, transaction))
                {
                    Console.WriteLine("Transfers success!");
                    Console.WriteLine("Continue with BlockChain? (y/n)");
                    if (Console.ReadLine().ToLower().Equals("n"))
                    {
                        Program.currentLoggedInAddress = null;
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