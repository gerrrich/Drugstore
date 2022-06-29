using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class DataBase
    {
        public ConnectionStringSettings ConnectionStringSettings { get; private set; }
        public readonly SqlConnection connection;
        public Queue<string> tasks;

        public DataBase()
        {
            tasks = new Queue<string>();

            if (ConfigurationManager.ConnectionStrings["Drugstore"] == null)
            {
                throw new Exception(); // todo: норм ошибка
            }

            ConnectionStringSettings = ConfigurationManager.ConnectionStrings["Drugstore"];

            connection = new SqlConnection(ConnectionStringSettings.ToString());

            try
            {
                connection.Open();
            }
            catch
            {
                throw; // todo: норм ошибка
            }
            finally
            {
                connection.Close();
            }
        }

        public void Fetch()
        {
            this.connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            SqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;

            try
            {
                while (tasks.Count > 0)
                {
                    string task = tasks.Dequeue();

                    command.CommandText = task;

                    int number = command.ExecuteNonQuery();
                    // Console.WriteLine("Добавлено объектов: {0}", number); логи
                }

                transaction.Commit();
                // logs
            }
            catch
            {
                transaction.Rollback();
                throw; // норм ошибку
            }
            finally
            {
                this.connection.Close();
            }
        }
    }
}
