using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Core
{
    public class DataBase : IDisposable
    {
        public ConnectionStringSettings ConnectionStringSettings { get; private set; }
        public readonly SqlConnection connection;
        public Queue<string> tasks;

        public DataBase()
        {
            tasks = new Queue<string>();

            if (ConfigurationManager.ConnectionStrings.Count == 0)
            {
                throw new Exception($"В App.Config отсутствует строка подключения");
            }

            foreach (ConnectionStringSettings css in ConfigurationManager.ConnectionStrings)
            {
                if (css.Name == "LocalSqlServer")
                {
                    continue;
                }

                ConnectionStringSettings = css;

                break;
            }

            connection = new SqlConnection(ConnectionStringSettings.ToString());

            try
            {
                connection.Open();
                Table.dataBase = this;
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось создать подключение к базе данных", ex);
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

        public List<List<IField>> SelectQuery(string selectQueryString)
        {
            this.connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            SqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;

            try
            {
                command.CommandText = selectQueryString;

                SqlDataReader reader = command.ExecuteReader();
                List<List<IField>> rows = new List<List<IField>>();

                while (reader.Read())
                {
                    List<IField> row = new List<IField>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add((IField)typeof(Field<>).MakeGenericType(reader.GetFieldType(i)).GetConstructor(new Type[] { reader.GetFieldType(i), typeof(string) }).Invoke(new object[] { reader.GetValue(i), reader.GetName(i) }));
                    }

                    rows.Add(row);
                }

                reader.Close();
                transaction.Commit();

                return rows;
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
            List<List<IField>> rows = SelectQuery(selectQueryString);

            Console.WriteLine();

            foreach (IField column in rows[0])
            {
                Console.Write(column.GetName() + "    ");
            }

            Console.WriteLine();

            foreach (List<IField> row in rows)
            {
                foreach (IField val in row)
                {
                    Console.Write(val.GetValue().ToString() + "    ");
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
