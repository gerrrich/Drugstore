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

        private static List<Storage> Unpack(List<List<object>> rows, List<string> columns, ref List<Store> stores)
        {
            if (stores == null)
            {
                stores = Store.Select();
            }

            bool isFirst = true;

        start:
            List<Storage> list = new List<Storage>();

            for (int i = 0; i < rows.Count; i++)
            {
                string name = null;
                string id = null;
                Store store = null;
                bool err = false;

                for (int j = 0; j < columns.Count; j++)
                {
                    if (columns[j].Equals("Name"))
                    {
                        name = rows[i][j] as string;
                    }
                    else if (columns[j].Equals("Store"))
                    {
                        string storeId = rows[i][j].ToString();
                        store = stores.Find((x) => x.Id == Guid.Parse(storeId));

                        if (store == null)
                        {
                            if (!isFirst)
                            {
                                err = true;
                            }

                            stores = Store.Select();

                            isFirst = false;
                            goto start;
                        }
                    }
                    else
                    {
                        id = rows[i][j].ToString();
                    }
                }

                if (err)
                {
                    continue;
                }

                Storage storage = Create(store, name, id);

                list.Add(storage);
            }

            return list;
        }

        public static List<Storage> Select(ref List<Store> stores)
        {
            (List<List<object>> rows, List<string> columns) = dataBase.SelectQuery($"SELECT * FROM {dataBase.ConnectionStringSettings.Name}.Storage");

            return Unpack(rows, columns, ref stores);
        }
    }
}
