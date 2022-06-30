using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Batch : Table
    {
        public Product Product { get; private set; }
        public Storage Storage { get; private set; }
        public short Count { get; private set; }

        public static Batch Create(Product product, Storage storage, short count, string id = null)
        {
            if (product == null)
            {
                throw new ArgumentException("Product не может быть пуст");
            }

            if (storage == null)
            {
                throw new ArgumentException("Storage не может быть пуст");
            }

            if (count < 1)
            {
                throw new ArgumentException("Count не может быть меньше одного");
            }

            Batch batch = new Batch()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                Product = product,
                Storage = storage,
                Count = count
            };

            return batch;
        }

        private static List<Batch> Unpack(List<List<object>> rows, List<string> columns, ref List<Product> products, ref List<Storage> storages, ref List<Store> stores)
        {
            if (products == null)
            {
                products = Product.Select();
            }

            if (stores == null)
            {
                stores = Store.Select();
            }

            if (storages == null)
            {
                storages = Storage.Select(ref stores);
            }

            bool isFirst = true;

        start:
            List<Batch> list = new List<Batch>();

            for (int i = 0; i < rows.Count; i++)
            {
                short count = 0;
                string id = null;
                Storage storage = null;
                Product product = null;
                bool err = false;

                for (int j = 0; j < columns.Count; j++)
                {
                    if (columns[j].Equals("Count"))
                    {
                        count = Convert.ToInt16(rows[i][j]);
                    }
                    else if (columns[j].Equals("Storage"))
                    {
                        string storageId = rows[i][j].ToString();
                        storage = storages.Find((x) => x.Id == Guid.Parse(storageId));

                        if (storage == null)
                        {
                            if (!isFirst)
                            {
                                err = true;
                            }

                            stores = Store.Select();
                            storages = Storage.Select(ref stores);
                            products = Product.Select();

                            isFirst = false;
                            goto start;
                        }
                    }
                    else if (columns[j].Equals("Product"))
                    {
                        string productId = rows[i][j].ToString();
                        product = products.Find((x) => x.Id == Guid.Parse(productId));

                        if (product == null)
                        {
                            if (!isFirst)
                            {
                                err = true;
                            }

                            stores = Store.Select();
                            storages = Storage.Select(ref stores);
                            products = Product.Select();

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

                Batch batch = Create(product, storage, count, id);

                list.Add(batch);
            }

            return list;
        }

        public static List<Batch> Select(ref List<Product> products, ref List<Storage> storages, ref List<Store> stores)
        {
            (List<List<object>> rows, List<string> columns) = dataBase.SelectQuery($"SELECT * FROM {dataBase.ConnectionStringSettings.Name}.Batch");

            return Unpack(rows, columns, ref products, ref storages, ref stores);
        }
    }
}
