using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Table
    {
        public Guid Id { get; protected set; }

        public static DataBase dataBase;

        protected Table() { }

        public static void CheckString(ref string str, bool isNullable = true, int size = 255)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                if (isNullable)
                {
                    str = null;
                    return;
                }

                throw new ArgumentException(nameof(str) + " не может быть пустым");
            }

            if (str.Length > size)
            {
                throw new ArgumentException(nameof(str) + " превышает максимальный размер строки хранимой в базе");
            }
        }

        public void Save()
        {
            if (dataBase == null)
            {
                throw new Exception("DataBase не может быть пустым");
            }

            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                if (propertyInfos[i].Name.Equals("Id"))
                {
                    names.Append(this.GetType().Name + "Id");
                }
                else
                {
                    names.Append(propertyInfos[i].Name);
                }

                object value = propertyInfos[i].GetValue(this);

                if (value == null)
                {
                    values.Append("NULL");
                }
                else if (value is string || value is Guid)
                {
                    values.Append($"'{value}'");
                }
                else if (value is Table t)
                {
                    values.Append($"'{t.Id}'");
                }
                else
                {
                    values.Append(value.ToString());
                }

                if (i != propertyInfos.Length - 1)
                {
                    names.Append(", ");
                    values.Append(", ");
                }
            }

            string sqlExpression = $"INSERT INTO {dataBase.ConnectionStringSettings.Name}.{this.GetType().Name} ({names}) VALUES ({values})";

            dataBase.tasks.Enqueue(sqlExpression);
        }

        public void Delete()
        {
            if (dataBase == null)
            {
                throw new Exception("DataBase не может быть пустым");
            }

            string guid = "";

            PropertyInfo[] propertyInfos = this.GetType().GetProperties();

            foreach (PropertyInfo pi in propertyInfos)
            {
                if (pi.PropertyType.Equals(typeof(Guid)) && pi.Name.Equals("Id"))
                {
                    guid = pi.GetValue(this).ToString();
                    break;
                }
            }

            string sqlExpression = $"DELETE FROM {dataBase.ConnectionStringSettings.Name}.{this.GetType().Name} WHERE {this.GetType().Name}Id = '{guid}'";

            dataBase.tasks.Enqueue(sqlExpression);
        }

        public static T SelectOneById<T>(string id)
        {
            return Select<T>($"WHERE {dataBase.ConnectionStringSettings.Name}.{typeof(T).Name}.{typeof(T).Name}Id = '{id}'", 1)[0];
        }

        public static List<T> Select<T>(string where = null, int top = -1)
        {
            List<T> list = new List<T>();
            List<List<object>> rows;
            List<string> columns;
            string query = "SELECT ";

            if (top != -1)
            {
                query += $"TOP({top}) ";
            }

            query += $"* FROM {dataBase.ConnectionStringSettings.Name}.{typeof(T).Name} ";

            if (!string.IsNullOrWhiteSpace(where))
            {
                query += where;
            }

            (rows, columns) = dataBase.SelectQuery(query);

            for (int i = 0; i < rows.Count; i++)
            {
                object[] parameters = new object[typeof(T).GetProperties().Length];

                for (int j = 0; j < columns.Count; j++)
                {
                    if (typeof(T).GetMethod("Create").GetParameters()[j].ParameterType.BaseType.Equals(typeof(Table)))
                    {
                        int ind = columns.FindIndex((x) => x.ToLower().Equals(typeof(T).GetMethod("Create").GetParameters()[j].Name));

                        parameters[j] = typeof(Table).GetMethod("SelectOneById").MakeGenericMethod(typeof(T).GetMethod("Create").GetParameters()[j].ParameterType).Invoke(null, new object[] { rows[i][ind].ToString() });
                    }
                    else
                    {
                        int ind = columns.FindIndex((x) => x.ToLower().Equals(typeof(T).GetMethod("Create").GetParameters()[j].Name));

                        if (ind == -1)
                        {
                            ind = columns.FindIndex((x) => x.Equals(typeof(T).Name + "Id"));
                        }

                        parameters[j] = rows[i][ind].ToString();
                    }
                }

                T t = (T)typeof(T).GetMethod("Create").Invoke(null, parameters);

                list.Add(t);
            }

            return list;
        }

        public static void Show<T>(List<T> list) where T : Table
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            StringBuilder names = new StringBuilder();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            Console.WriteLine();


            for (int i = 0; i < propertyInfos.Length; i++)
            {
                if (propertyInfos[i].Name.Equals("Id"))
                {
                    names.Append(typeof(T).Name + "Id");
                }
                else
                {
                    names.Append(propertyInfos[i].Name);
                }

                if (i != propertyInfos.Length - 1)
                {
                    names.Append("    ");
                }
            }

            Console.WriteLine(names);

            for (int j = 0; j < list.Count; j++)
            {
                StringBuilder values = new StringBuilder(j.ToString());
                values.Append("    ");

                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    object value = propertyInfos[i].GetValue(list[j]);

                    if (value == null)
                    {
                        values.Append("Пусто");
                    }
                    else if (value is string || value is Guid)
                    {
                        values.Append($"'{value}'");
                    }
                    else if (value is Table t)
                    {
                        values.Append($"'{t.Id}'");
                    }
                    else
                    {
                        values.Append(value.ToString());
                    }

                    if (i != propertyInfos.Length - 1)
                    {
                        values.Append("    ");
                    }
                }

                Console.WriteLine(values);
            }
        }
    }
}
