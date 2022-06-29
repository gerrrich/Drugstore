using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Reflection;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            DataBase dataBase = new DataBase();

            Store store = Store.CreateStore("kek", "", "");
            Storage storage = Storage.CreateStorage(store, "kek1");
            Product product = Product.CreateProduct("kek2");
            Batch batch = Batch.CreateBatch(product, storage, 20);

            store.Save(dataBase);
            storage.Save(dataBase);
            product.Save(dataBase);
            batch.Save(dataBase);

            dataBase.Fetch();

            batch.Delete(dataBase);
            product.Delete(dataBase);
            storage.Delete(dataBase);
            store.Delete(dataBase);

            dataBase.Fetch();

            while (true)
            {
                Console.WriteLine("Введите команду (для получения списка команд введите \"h\"):");

                string command = Console.ReadLine();

                switch (command)
                {
                    case "h":
                        Console.WriteLine("Список команд:\nВыход: \"exit\"\n");
                        continue;

                    case "exit":
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда.\n");
                        continue;
                }

                // перед выходом
                break;
            }
        }
    }
}
