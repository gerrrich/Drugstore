using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Product : Table
    {
        public string Name { get; private set; }

        private Product() { }

        public static Product Create(string name, string id = null)
        {
            CheckString(ref name, false);

            Product product = new Product()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                Name = name
            };

            return product;
        }

        private static List<Product> Unpack(List<List<object>> rows, List<string> columns)
        {
            List<Product> list = new List<Product>();

            for (int i = 0; i < rows.Count; i++)
            {
                string name = null;
                string id = null;

                for (int j = 0; j < columns.Count; j++)
                {
                    if (columns[j].Equals("Name"))
                    {
                        name = rows[i][j] as string;
                    }
                    else
                    {
                        id = rows[i][j].ToString();
                    }
                }

                Product product = Create(name, id);

                list.Add(product);
            }

            return list;
        }

        public static List<Product> Select()
        {
            (List<List<object>> rows, List<string> columns) = dataBase.SelectQuery($"SELECT * FROM {dataBase.ConnectionStringSettings.Name}.Product");

            return Unpack(rows, columns);
        }
    }
}
