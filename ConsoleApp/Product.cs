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

        public static Product CreateProduct(string name)
        {
            CheckString(ref name, false);

            Product product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            return product;
        }
    }
}
