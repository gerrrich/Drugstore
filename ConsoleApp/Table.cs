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

                throw new ArgumentException(nameof(str));// норм ошибка
            }

            if (str.Length > size)
            {
                throw new ArgumentException();// норм ошибка
            }
        }

        public void Save(DataBase dataBase)
        {
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

        public void Delete(DataBase dataBase)
        {
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
    }
}
