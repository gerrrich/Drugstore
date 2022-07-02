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

        public static Storage SelectOneById(string id)
        {
            return SelectOneById<Storage>(id);
        }

        public static Storage CreateWithOld(Storage old, params (string name, object value)[] parameters)
        {
            return CreateWithOld<Storage>(old, parameters);
        }

        public static Storage Create(Store store, string name, string id = null)
        {
            CheckString(ref name, false);

            if (store == null)
            {
                throw new ArgumentException("Store не может быть пустым");
            }

            Storage storage = new Storage()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                Store = store,
                Name = name
            };

            return storage;
        }
    }
}
