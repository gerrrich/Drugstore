using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Store : Table
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string Phone { get; private set; }

        private Store() { }

        public static Store Create(string name, string address, string phone, string id = null)
        {
            CheckString(ref name, false);
            CheckString(ref phone);
            CheckString(ref address, true, 500);

            if (phone != null)
            {
                phone = phone.Replace("+", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");

                if (phone[0] == '7')
                {
                    phone = "8" + phone.Remove(0, 1);
                }

                if (phone.Length != 11 && phone.Length != 7)
                {
                    throw new ArgumentException("Неверный формат номера телефона");
                }
            }

            Store store = new Store()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                Name = name,
                Address = address,
                Phone = phone
            };

            return store;
        }

        private static List<Store> Unpack(List<List<object>> rows, List<string> columns)
        {
            List<Store> list = new List<Store>();

            for (int i = 0; i < rows.Count; i++)
            {
                string name = null;
                string address = null;
                string phone = null;
                string id = null;

                for (int j = 0; j < columns.Count; j++)
                {
                    if (columns[j].Equals("Name"))
                    {
                        name = rows[i][j] as string;
                    }
                    else if (columns[j].Equals("Address"))
                    {
                        address = rows[i][j] as string;
                    }
                    else if (columns[j].Equals("Phone"))
                    {
                        phone = rows[i][j] as string;
                    }
                    else
                    {
                        id = rows[i][j].ToString();
                    }
                }

                Store store = Create(name, address, phone, id);

                list.Add(store);
            }

            return list;
        }

        public static List<Store> Select()
        {
            (List<List<object>> rows, List<string> columns) = dataBase.SelectQuery($"SELECT * FROM {dataBase.ConnectionStringSettings.Name}.Store");

            return Unpack(rows, columns);
        }
    }
}
