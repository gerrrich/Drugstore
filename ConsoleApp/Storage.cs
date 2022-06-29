using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Storage : Table
    {
        public Store Store { get; private set; }
        public string Name { get; private set; }

        private Storage() { }

        public static Storage CreateStorage(Store store, string name)
        {
            CheckString(ref name, false);

            if (store == null)
            {
                throw new ArgumentException();//норм ошибка
            }

            Storage storage = new Storage()
            {
                Id = Guid.NewGuid(),
                Store = store,
                Name = name
            };

            return storage;
        }
    }
}
