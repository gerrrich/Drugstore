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

        public static Batch CreateBatch(Product product, Storage storage, short count)
        {
            if (product == null)
            {
                throw new ArgumentException();// норм ошибку
            }

            if (storage == null)
            {
                throw new ArgumentException();// норм ошибку
            }

            if (count <= 0)
            {
                throw new ArgumentException(); // норм ошибку
            }

            Batch batch = new Batch()
            {
                Id = Guid.NewGuid(),
                Product = product,
                Storage = storage,
                Count = count
            };

            return batch;
        }
    }
}
