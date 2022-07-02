using System;
using Core;

namespace Tables
{
    internal class Batch : Table
    {
        #region fields
        public Product Product
        {
            get
            {
                return _product.Value;
            }
            set
            {
                _product.Set(value);
            }
        }
        private Field<Product> _product;

        public Storage Storage
        {
            get
            {
                return _storage.Value;
            }
            set
            {
                _storage.Set(value);
            }
        }
        private Field<Storage> _storage;

        public short Count
        {
            get
            {
                return _count.Value;
            }
            set
            {
                _count.Set(value);
            }
        }
        private Field<short> _count;
        #endregion fields

        private Batch() : base() { }

        public static Batch SelectOneById(string id)
        {
            return SelectOneById<Batch>(id);
        }

        public static Batch CreateWithOld(Batch old, params (string name, object value)[] parameters)
        {
            return CreateWithOld<Batch>(old, parameters);
        }

        public static Batch Create(Product product, Storage storage, short count, string id = null)
        {
            Batch batch = new Batch()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                _product = new Field<Product>(product, nameof(Product)),
                _storage = new Field<Storage>(storage, nameof(Storage)),
                _count = new Field<short>(count, nameof(Count))
            };

            return batch;
        }
    }
}
