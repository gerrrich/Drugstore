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
    }
}
