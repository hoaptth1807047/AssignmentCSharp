using System;
using MySql.Data.MySqlClient;
using SunBank.entity;

namespace SunBank.model
{
    public class BlockChainAddressModel
    {
        public BlockChainAddress FindByAddressAndPrivateKey(string address, string privateKey)
        {
            var cmd = new MySqlCommand("select * from address where address = @address and privateKey = @privateKey", 
                ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@privateKey", privateKey);
            var reader = cmd.ExecuteReader();
            var blockchainAddress = new BlockChainAddress();
            if (reader.Read())
            {
                blockchainAddress.Address = reader.GetString("address");
                blockchainAddress.PrivateKey = reader.GetString("privateKey");
                blockchainAddress.Balance = reader.GetDouble("balance");
            }
            
            ConnectionHelper.CloseConnection();
            return blockchainAddress;
        }

        public bool UpdateBalance(BlockChainAddress currentLoggedInAddress, TransactionBlockChain transactionBc)
        {
            var trans = ConnectionHelper.GetConnection().BeginTransaction();
            try 
            {
                var cmd = new MySqlCommand("select balance from address where address = @address", 
                    ConnectionHelper.GetConnection());
                cmd.Parameters.AddWithValue("@address", currentLoggedInAddress.Address);
                var reader = cmd.ExecuteReader();
                double currentBalance = 0;
                if (reader.Read())
                {
                    currentBalance = reader.GetDouble("balance");
                }
                reader.Close();
                if (transactionBc.Type == TransactionBlockChain.TransactionType.Withdraw&&
                    currentBalance < transactionBc.Amount)
                {
                    throw new Exception("Not enough money in the account.");
                }

                if (transactionBc.Type == TransactionBlockChain.TransactionType.Withdraw)
                {
                    currentBalance -= transactionBc.Amount;
                }
                
                if (transactionBc.Type == TransactionBlockChain.TransactionType.Deposit)
                {
                    currentBalance += transactionBc.Amount;
                }
                
                var cmd1 = new MySqlCommand(
                    "update address set balance = @balance WHERE address = @address", 
                    ConnectionHelper.GetConnection());
                cmd1.Parameters.AddWithValue("@balance", currentBalance);
                cmd1.Parameters.AddWithValue("@address", currentLoggedInAddress.Address);
                var updateResult = cmd1.ExecuteNonQuery();
                
                var cmd2 = new MySqlCommand(
                    "insert into blockchaintransactions (transactionId, senderAddress, receiverAddress, type, amount, createdAt, updatedAt, status) values ( @transactionId, @senderAddress, @receiverAddress, @type, @amount, @createdAt, @updatedAt, @status) ", 
                    ConnectionHelper.GetConnection());
                cmd2.Parameters.AddWithValue("@transactionId", transactionBc.TransactionId);
                cmd2.Parameters.AddWithValue("@senderAddress", transactionBc.SenderAddress);
                cmd2.Parameters.AddWithValue("@receiverAddress", transactionBc.ReceiveAddress);
                cmd2.Parameters.AddWithValue("@type", transactionBc.Type);
                cmd2.Parameters.AddWithValue("@amount", transactionBc.Amount);
                cmd2.Parameters.AddWithValue("@createdAt", transactionBc.CreatedAtMls);
                cmd2.Parameters.AddWithValue("@updatedAt", transactionBc.UpdatedAtMls);
                cmd2.Parameters.AddWithValue("@status", transactionBc.Status);
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

        public bool Transfer(BlockChainAddress currentLoggedInAddress, TransactionBlockChain transactionBc)
        {
            var trans = ConnectionHelper.GetConnection().BeginTransaction();

            try
            {
                var cmd = new MySqlCommand("select balance from address where address = @address",
                    ConnectionHelper.GetConnection());
                cmd.Parameters.AddWithValue("@address", currentLoggedInAddress.Address);
                var reader = cmd.ExecuteReader();
                double currentBalance = 0;
                if (reader.Read())
                {
                    currentBalance = reader.GetDouble("balance");
                }
                reader.Close(); 
                if (currentBalance < transactionBc.Amount)
                {
                    throw new Exception("Not enough money.");
                }

                currentBalance -= transactionBc.Amount;

                var cmd1 = new MySqlCommand("update address set balance = @balance where address = @address",
                    ConnectionHelper.GetConnection());
                cmd1.Parameters.AddWithValue("@balance", currentBalance);
                cmd1.Parameters.AddWithValue("@address", currentLoggedInAddress.Address);
                
                var updateResult = cmd1.ExecuteNonQuery();
                Console.WriteLine(currentLoggedInAddress.Address);
                Console.WriteLine(transactionBc.ReceiveAddress);

                
                var cmd2 = new MySqlCommand("select balance from address where address = @address",
                    ConnectionHelper.GetConnection());
                cmd2.Parameters.AddWithValue("@address", transactionBc.ReceiveAddress);
                var reader1 = cmd2.ExecuteReader();
                double receiverBalance = 0;
                if (reader1.Read())
                {
                    receiverBalance = reader1.GetDouble("balance");
                }

                reader1.Close();                
                receiverBalance += transactionBc.Amount;
                
                var cmd3 = new MySqlCommand("update accounts set balance = @balance where address = @address",
                    ConnectionHelper.GetConnection());
                cmd3.Parameters.AddWithValue("@balance", receiverBalance);
                cmd3.Parameters.AddWithValue("@address", transactionBc.ReceiveAddress);
                var updateResult1 = cmd3.ExecuteNonQuery();

                var cmd4 = new MySqlCommand(
                    "insert into blockchaintransactions (transactionId, senderAddress, receiverAddress, type, amount, createdAt, updatedAt, status) values ( @transactionId, @senderAddress, @receiverAddress, @type, @amount, @createdAt, @updatedAt, @status) ", 
                    ConnectionHelper.GetConnection());
                cmd4.Parameters.AddWithValue("@transactionId", transactionBc.TransactionId);
                cmd4.Parameters.AddWithValue("@senderAddress", transactionBc.SenderAddress);
                cmd4.Parameters.AddWithValue("@receiverAddress", transactionBc.ReceiveAddress);
                cmd4.Parameters.AddWithValue("@type", transactionBc.Type);
                cmd4.Parameters.AddWithValue("@amount", transactionBc.Amount);
                cmd4.Parameters.AddWithValue("@createdAt", transactionBc.CreatedAtMls);
                cmd4.Parameters.AddWithValue("@updatedAt", transactionBc.UpdatedAtMls);
                cmd4.Parameters.AddWithValue("@status", transactionBc.Status);
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