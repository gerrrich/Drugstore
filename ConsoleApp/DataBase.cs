using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class DataBase : IDisposable
    {
        public ConnectionStringSettings ConnectionStringSettings { get; private set; }
        public readonly SqlConnection connection;
        public Queue<string> tasks;

        public DataBase()
        {
            tasks = new Queue<string>();

            if (ConfigurationManager.ConnectionStrings["Drugstore"] == null)
            {
                throw new Exception($"В App.Config отсутствует строка подключения \"Drugstore\"");
            }

            ConnectionStringSettings = ConfigurationManager.ConnectionStrings["Drugstore"];

            connection = new SqlConnection(ConnectionStringSettings.ToString());

            try
            {
                connection.Open();
                Table.dataBase = this;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось создать пробное подключение к базе", ex);
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
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Не удалось выполнить транзакцию", ex);
            }
            finally
            {
                this.connection.Close();
            }
        }

        public (List<List<object>> rows, List<string> columns) SelectQuery(string selectQueryString)
        {
            this.connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            SqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;
            SqlDataReader reader = null;

            try
            {
                command.CommandText = selectQueryString;

                reader = command.ExecuteReader();

                List<List<object>> rows = new List<List<object>>();
                List<string> columns = new List<string>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    columns.Add(reader.GetName(i));
                }

                while (reader.Read())
                {
                    List<object> row = new List<object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetValue(i));
                    }

                    rows.Add(row);
                }

                reader.Close();
                transaction.Commit();

                return (rows, columns);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Не удалось выполнить транзакцию", ex);
            }
            finally
            {
                this.connection.Close();
            }
        }

        public void ShowSelectQuery(string selectQueryString)
        {
            (List<List<object>> rows, List<string> columns) = SelectQuery(selectQueryString);

            Console.WriteLine();

            foreach (string column in columns)
            {
                Console.Write(column + "    ");
            }

            Console.WriteLine();

            foreach (List<object> row in rows)
            {
                foreach (object val in row)
                {
                    Console.Write(val.ToString() + "    ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public void Dispose()
        {
            Table.dataBase = null;
            connection.Close();
        }
    }
}
