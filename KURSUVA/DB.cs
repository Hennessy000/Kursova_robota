using System;
using System.Data;
using System.Data.SqlClient;

namespace KURSUVA
{
    internal class DB
    {
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-ONBL7EE;Initial Catalog=BDB;Integrated Security=True");

        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public SqlConnection getConnection()
        {
            return connection;
        }
        public int GetUserIdByPhoneNumber(string phoneNumber)
        {
            int userId = -1;
            try
            {
                OpenConnection();

                string query = "SELECT id_users FROM users WHERE numberPhone = @phoneNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out userId))
                {
                    return userId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return userId;
        }
        public string GetCardNumberFromDatabase(int userId)
        {
            string cardNumber = null;
            try
            {
                OpenConnection();

                string query = "SELECT CardNumber FROM bank_cards WHERE id_users = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

              
                object result = command.ExecuteScalar();

               
                if (result != null)
                {
                    cardNumber = result.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return cardNumber;
        }

        public int GetUserIdByCardNumber(string cardNumber)
        {
            int userId = -1;
            try
            {
                OpenConnection();

                string query = "SELECT id_users FROM bank_cards WHERE CardNumber = @cardNumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cardNumber", cardNumber);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    userId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUserIdByCardNumber Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return userId;
        }

        public decimal GetBalanceFromDatabase(int userId)
        {
            decimal balance = 0;
            try
            {
                OpenConnection();

                string query = "SELECT balance FROM bank_cards WHERE id_users = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    balance = Convert.ToDecimal(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetBalanceFromDatabase Error: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return balance;
        }


        public string GetUserNameFromDatabase(int userId)
        {
            string userName = null;
            try
            {
                OpenConnection();

                string query = "SELECT name, surname FROM users WHERE id_users = @userId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string name = reader.GetString(reader.GetOrdinal("name"));
                    string surname = reader.GetString(reader.GetOrdinal("surname"));
                    userName = $"{name} {surname}";
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return userName;
        }


        public bool Transaction(int senderUserId, int recipientUserId, decimal amount)
        {
            try
            {
                OpenConnection();

                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    decimal senderBalance = GetBalanceFromDatabase(senderUserId);
                    Console.WriteLine($"Sender Balance: {senderBalance}, Amount: {amount}");

                    if (senderBalance >= amount)
                    {
                        string updateSenderQuery = "UPDATE bank_cards SET balance = balance - @amount WHERE id_users = @userId";
                        ExecuteCommand(updateSenderQuery, transaction, new SqlParameter("@amount", amount), new SqlParameter("@userId", senderUserId));

                        string updateRecipientQuery = "UPDATE bank_cards SET balance = balance + @amount WHERE id_users = @userId";
                        ExecuteCommand(updateRecipientQuery, transaction, new SqlParameter("@amount", amount), new SqlParameter("@userId", recipientUserId));

                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully.");
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        Console.WriteLine("Insufficient funds for the transaction.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transaction Error: " + ex.Message);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        public bool RecordTransaction(int senderUserId, int recipientUserId, decimal amount)
        {
            try
            {
                OpenConnection();

                string query = "INSERT INTO transaction (sender_user_id, recipient_user_id, amount) VALUES (@senderUserId, @recipientUserId, @amount)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@senderUserId", senderUserId);
                command.Parameters.AddWithValue("@recipientUserId", recipientUserId);
                command.Parameters.AddWithValue("@amount", amount);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("RecordTransaction Error: " + ex.Message);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }


        private void ExecuteCommand(string query, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddRange(parameters);
                command.ExecuteNonQuery();
            }
        }
    }
}