using System;
using SunBank.entity;

namespace SunBank
{
    class Program
    {
        public static BlockChainAddress currentLoggedInAddress;
        public static SBAccount currentLoggedInAccount;
        static void Main(string[] args)
        {
            while (true)
            {
                Transaction transaction = null;
                Console.WriteLine("1. SunBank Transaction.");
                Console.WriteLine("2. BlockChain Transaction.");
                Console.WriteLine("3. Close.");
                Console.WriteLine("Please enter your choice: ");
                var choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        transaction = new SBTransaction();
                        break;
                    case 2:
                        transaction = new BlockChainTransaction();
                        break;
                    case 3:
                        Console.WriteLine("Closed Application!");
                        break;
                    default:
                        Console.WriteLine("Wrong login method. Please choose from 1 to 4.");
                        break;
                }

                transaction.Login();
                if (currentLoggedInAccount != null)
                {
                    GenerateMenu(transaction);
                }

                if (choice != 3) continue;
                Console.WriteLine("See you again.");
                break;
            }
            
        }

        public static void GenerateMenu(Transaction transaction)
        {
            currentLoggedInAddress = null;
            currentLoggedInAccount = null;
            Console.Clear();
            Console.WriteLine("Please choose the type of transaction: ");
            Console.WriteLine("1. Withdraw.");
            Console.WriteLine("2. Deposit.");
            Console.WriteLine("3. Transfer.");
            Console.WriteLine("Please enter your choice: ");
            var choice = int.Parse(Console.ReadLine());
            while (true)
            {
                switch (choice)
                {
                    case 1:
                        transaction.Withdraw();
                        break;
                    case 2:
                        transaction.Deposit();
                        break;
                    case 3:
                        transaction.Transfer();
                        break;
                    default:
                        Console.WriteLine("Choose the wrong type of transaction.");
                        break;
                }
                if (choice == 4)
                {
                    break;
                }
            }
            
        }
    }

    internal interface Transaction
    {
        void Withdraw();
        void Deposit();
        void Transfer();
        void Login();
    }
}