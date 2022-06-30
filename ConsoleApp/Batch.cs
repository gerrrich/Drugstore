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

        public static Batch Create(Product product, Storage storage, string count, string id = null)
        {
            if (product == null)
            {
                throw new ArgumentException("Product не может быть пуст");
            }

            if (storage == null)
            {
                throw new ArgumentException("Storage не может быть пуст");
            }

            short countS = short.Parse(count);

            if (countS < 1)
            {
                throw new ArgumentException("Count не может быть меньше одного");
            }

            Batch batch = new Batch()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                Product = product,
                Storage = storage,
                Count = countS
            };

            return batch;
        }
    }
}
