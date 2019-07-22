using System;
using MySql.Data.MySqlClient;
using SunBank.entity;

namespace SunBank.model
{
    public class SBAccountModel
    {
        public SBAccount FindByUserNameAndPassword(string userName, string password)
        {
            var cmd = new MySqlCommand("select * from accounts where userName = @userName and password = @password", 
                ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@userName", userName);
            cmd.Parameters.AddWithValue("@password", password);
            var reader = cmd.ExecuteReader();
            var sbAccount = new SBAccount();
            if (reader.Read())
            {
                sbAccount.UserName = reader.GetString("userName");
                sbAccount.Password = reader.GetString("password");
                sbAccount.Balance = reader.GetDouble("balance");
            }
            
            ConnectionHelper.CloseConnection();
            return sbAccount;
        }

        public bool UpdateBalance(SBAccount currentLoggedInAccount, TransactionSB transactionSb)
        {
            var trans = ConnectionHelper.GetConnection().BeginTransaction();
            try 
            {
                var cmd = new MySqlCommand("select balance from accounts where accountNumber = @accountNumber", ConnectionHelper.GetConnection());
                cmd.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var reader = cmd.ExecuteReader();
                double currentBalance = 0;
                if (reader.Read())
                {
                    currentBalance = reader.GetDouble("balance");
                }
                reader.Close();
                if (transactionSb.Type == TransactionSB.TransactionType.Withdraw&&
                    currentBalance < transactionSb.Amount)
                {
                    throw new Exception("Not enough money in the account.");
                }

                if (transactionSb.Type == TransactionSB.TransactionType.Withdraw)
                {
                    currentBalance -= transactionSb.Amount;
                }
                
                if (transactionSb.Type == TransactionSB.TransactionType.Deposit)
                {
                    currentBalance += transactionSb.Amount;
                }
                
                var cmd1 = new MySqlCommand(
                    "update accounts set balance = @balance where accountNumber = @accountNumber", 
                    ConnectionHelper.GetConnection());
                cmd1.Parameters.AddWithValue("@balance", currentBalance);
                cmd1.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var updateResult = cmd1.ExecuteNonQuery();
                
                var cmd2 = new MySqlCommand(
                    "insert into transactions (transactionId, senderAccountNumber, receiverAccountNumber, type, amount, message, createdAt, updatedAt, status) values ( @transactionId, @senderAccountNumber, @receiverAccountNumber, @type, @amount, @message, @createdAt, @updatedAt, @status) ", 
                    ConnectionHelper.GetConnection());
                cmd2.Parameters.AddWithValue("@transactionId", transactionSb.TransactionId);
                cmd2.Parameters.AddWithValue("@senderAccountNumber", transactionSb.SenderAccountNumber);
                cmd2.Parameters.AddWithValue("@receiverAccountNumber", transactionSb.ReceiverAccountNumber);
                cmd2.Parameters.AddWithValue("@type", transactionSb.Type);
                cmd2.Parameters.AddWithValue("@amount", transactionSb.Amount);
                cmd2.Parameters.AddWithValue("@message", transactionSb.Message);
                cmd2.Parameters.AddWithValue("@createdAt", transactionSb.CreatedAtMls);
                cmd2.Parameters.AddWithValue("@updatedAt", transactionSb.UpdatedAtMls);
                cmd2.Parameters.AddWithValue("@status", transactionSb.Status);
                var transactionResult = cmd2.ExecuteNonQuery();
                
                if (updateResult != 1 || transactionResult != 1)
                {
                    throw new Exception("Cannot add transactions or update accounts.");
                }
                trans.Commit();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.WriteLine(e);
                return false;
            }
            finally
            {                
                ConnectionHelper.CloseConnection();
            }
        }

        public bool Transfer(SBAccount currentLoggedInAccount, TransactionSB transactionSb)
        {
            var trans = ConnectionHelper.GetConnection().BeginTransaction();

            try
            {
                var cmd = new MySqlCommand("select balance from accounts where accountNumber = @accountNumber",
                    ConnectionHelper.GetConnection());
                cmd.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var reader = cmd.ExecuteReader();
                double currentBalance = 0;
                if (reader.Read())
                {
                    currentBalance = reader.GetDouble("balance");
                }
                reader.Close(); 
                if (currentBalance < transactionSb.Amount)
                {
                    throw new Exception("Not enough money.");
                }

                currentBalance -= transactionSb.Amount;

                var cmd1 = new MySqlCommand("update accounts set balance = @balance where accountNumber = @accountNumber",
                    ConnectionHelper.GetConnection());
                cmd1.Parameters.AddWithValue("@balance", currentBalance);
                cmd1.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                
                var updateResult = cmd1.ExecuteNonQuery();
                Console.WriteLine(currentLoggedInAccount.AccountNumber);
                Console.WriteLine(transactionSb.ReceiverAccountNumber);

                
                var cmd2 = new MySqlCommand("select balance from account where accountNumber = @accountNumber",
                    ConnectionHelper.GetConnection());
                cmd2.Parameters.AddWithValue("@accountNumber", transactionSb.ReceiverAccountNumber);
                var reader1 = cmd2.ExecuteReader();
                double receiverBalance = 0;
                if (reader1.Read())
                {
                    receiverBalance = reader1.GetDouble("balance");
                }

                reader1.Close();                
                receiverBalance += transactionSb.Amount;
                
                var cmd3 = new MySqlCommand("update accounts set balance = @balance where accountNumber = @accountNumber",
                    ConnectionHelper.GetConnection());
                cmd3.Parameters.AddWithValue("@balance", receiverBalance);
                cmd3.Parameters.AddWithValue("@accountNumber", transactionSb.ReceiverAccountNumber);
                var updateResult1 = cmd3.ExecuteNonQuery();

                var cmd4 = new MySqlCommand(
                    "insert into transactions (transactionId, senderAccountNumber, receiverAccountNumber, type, amount, message, createdAt, updatedAt, status) values ( @transactionId, @senderAccountNumber, @receiverAccountNumber, @type, @amount, @message, @createdAt, @updatedAt, @status) ", 
                    ConnectionHelper.GetConnection());
                cmd4.Parameters.AddWithValue("@transactionId", transactionSb.TransactionId);
                cmd4.Parameters.AddWithValue("@senderAccountNumber", transactionSb.SenderAccountNumber);
                cmd4.Parameters.AddWithValue("@receiverAccountNumber", transactionSb.ReceiverAccountNumber);
                cmd4.Parameters.AddWithValue("@type", transactionSb.Type);
                cmd4.Parameters.AddWithValue("@amount", transactionSb.Amount);
                cmd4.Parameters.AddWithValue("@message", transactionSb.Message);
                cmd4.Parameters.AddWithValue("@createdAt", transactionSb.CreatedAtMls);
                cmd4.Parameters.AddWithValue("@updatedAt", transactionSb.UpdatedAtMls);
                cmd4.Parameters.AddWithValue("@status", transactionSb.Status);
                var transactionResult = cmd4.ExecuteNonQuery();

                if (updateResult != 1 || transactionResult != 1 || updateResult1 != 1)
                {
                    throw new Exception("Cannot add transactions or update accounts.");
                }

                trans.Commit();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                Console.WriteLine(e);
                return false;
            }
            finally
            {
                ConnectionHelper.CloseConnection();
            }
        }
    }
}