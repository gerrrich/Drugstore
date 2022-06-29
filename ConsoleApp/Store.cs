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

        public static Store CreateStore(string name, string address, string phone)
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
                    throw new ArgumentException();// норм ошибка
                }
            }

            Store store = new Store()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Address = address,
                Phone = phone
            };

            return store;
        }
    }
}
