using System;
using System.Collections.Generic;
using Core;
using Tables;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            using (DataBase dataBase = new DataBase())
            {
                dataBase.ShowSelectQuery("select * from Drugstore.Product");

                while (true)
                {
                    Console.WriteLine("Введите команду (для получения списка команд введите \"help\"):");

                    string command = Console.ReadLine();

                    try
                    {
                        if (!ExecCommand(command, dataBase))
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Console.WriteLine();
                    }
                }
            }
        }

        static bool Choise(string question, int maxNum, out int number)
        {
            number = -1;
            bool isExit = false;

            while (true)
            {
                Console.Write($"\nВыберите {question} (введите номер): ");
                string numStr = Console.ReadLine();

                if (numStr.Equals("exit"))
                {
                    isExit = true;
                    break;
                }

                if (!int.TryParse(numStr, out number) || number < 0 || number > maxNum)
                {
                    Console.WriteLine("Не верно выбран номер, попробуйте еще раз (чтобы выйти напишите \"exit\")");
                    continue;
                }

                break;
            }

            return isExit;
        }

        static bool DeleteCascade<T>(DataBase dataBase, string name) where T : Table
        {
            List<T> ts = Table.Select<T>();

            Table.Show(ts);

            if (Choise(name, ts.Count, out int number))
            {
                return false;
            }

            ts[number].Delete();
            dataBase.Fetch();

            return true;
        }

        static bool ExecCommand(string command, DataBase dataBase)
        {
            switch (command)
            {
                case "show":
                    {
                        if (Choise("что отобразить? Товар - 0, Аптека - 1, Склад - 2, Партия - 3", 3, out int number))
                        {
                            return true;
                        }

                        ExecCommand("show " + number, dataBase);

                        return true;
                    }
                case "show 0": // Товар
                    Table.Show(Table.Select<Product>());
                    return true;
                case "show 1": // Аптека
                    Table.Show(Table.Select<Store>());
                    return true;
                case "show 2": // Склад
                    Table.Show(Table.Select<Storage>());
                    return true;
                case "show 3": // Партия
                    Table.Show(Table.Select<Batch>());
                    return true;
                case "del":
                    {
                        if (Choise("что удалить? Товар - 0, Аптека - 1, Склад - 2, Партия - 3", 3, out int number))
                        {
                            return true;
                        }

                        ExecCommand("del " + number, dataBase);

                        return true;
                    }
                case "del 0": // Товар
                    DeleteCascade<Product>(dataBase, "товар");
                    return true;

                case "del 1": // Аптека
                    DeleteCascade<Store>(dataBase, "аптеку");
                    return true;

                case "del 2": // Склад
                    DeleteCascade<Storage>(dataBase, "аптеку");
                    return true;

                case "del 3": // Партия
                    DeleteCascade<Batch>(dataBase, "партию");
                    return true;

                case "new":
                    {
                        if (Choise("что создать? Товар - 0, Аптека - 1, Склад - 2, Партия - 3", 3, out int number))
                        {
                            return true;
                        }

                        ExecCommand("new " + number, dataBase);

                        return true;
                    }
                case "new 0": // Товар
                    {
                        Console.Write("Создание товара...\nВведите имя товара: ");

                        Product product = Product.Create(Console.ReadLine());

                        Console.WriteLine();

                        product.Save();
                        dataBase.Fetch();

                        return true;
                    }
                case "new 1": // Аптека
                    {
                        Console.WriteLine("Создание аптеки...");

                        Console.WriteLine("Введите имя аптеки: ");
                        string name = Console.ReadLine();

                        Console.WriteLine("Введите адрес аптеки: ");
                        string address = Console.ReadLine();

                        Console.WriteLine("Введите телефон аптеки: ");
                        string phone = Console.ReadLine();

                        Store store = Store.Create(name, address, phone);

                        store.Save();
                        dataBase.Fetch();

                        return true;
                    }
                case "new 2": // Склад
                    {
                        Console.WriteLine("Создание склада...");

                        Console.WriteLine("Введите название склада: ");
                        string name = Console.ReadLine();

                        List<Store> stores = Table.Select<Store>();

                        Table.Show(stores);

                        if (Choise("аптеку", stores.Count, out int number))
                        {
                            return true;
                        }

                        Storage storage = Storage.Create(stores[number], name);

                        Console.WriteLine();

                        storage.Save();
                        dataBase.Fetch();

                        return true;
                    }
                case "new 3": // Партия
                    {
                        Console.WriteLine("Создание партии...");

                        Console.WriteLine("Введите количество товара в партии: ");
                        string count = Console.ReadLine();

                        List<Product> products = Table.Select<Product>();
                        List<Storage> storages = Table.Select<Storage>();

                        Table.Show(products);

                        if (Choise("товар", products.Count, out int number1))
                        {
                            return true;
                        }

                        Table.Show(storages);

                        if (Choise("склад", storages.Count, out int number2))
                        {
                            return true;
                        }

                        Batch batch = Batch.Create(products[number1], storages[number2], short.Parse(count));

                        Console.WriteLine();

                        batch.Save();
                        dataBase.Fetch();

                        return true;
                    }
                case "pts":
                    {
                        List<Store> stores = Table.Select<Store>();
                        Table.Show(stores);

                        if (Choise("аптеку", stores.Count, out int number))
                        {
                            return true;
                        }

                        dataBase.ShowSelectQuery($"SELECT p.Name AS 'ProductName', SUM(b.[Count]) AS 'ProductCount' " +
                            $"FROM {dataBase.ConnectionStringSettings.Name}.Batch b " +
                            $"JOIN {dataBase.ConnectionStringSettings.Name}.Product p ON b.Product = p.ProductId " +
                            $"JOIN {dataBase.ConnectionStringSettings.Name}.Storage sa ON b.Storage = sa.StorageId " +
                            $"JOIN {dataBase.ConnectionStringSettings.Name}.Store st ON sa.Store = st.StoreId WHERE sa.Store = '{stores[number].Id}'" +
                            $"GROUP BY p.Name");

                        return true;
                    }
                case "help":
                    Console.WriteLine("\nСписок команд:\n* Выход: \"exit\"\n* Cписок товаров: \"pts\"\n* Создать: \"new\"\n* Удалить: \"del\"\n* Показать таблицу: \"show\"\n");
                    return true;
                case "exit":
                    return false;
                default:
                    Console.WriteLine("Неизвестная команда.\n");
                    return true;
            }
        }
    }
}
