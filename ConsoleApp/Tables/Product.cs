using System;
using Core;

namespace Tables
{
    internal class Product : Table
    {
        #region fields
        public string Name
        {
            get
            {
                return _name.Value;
            }
            set
            {
                _name.Set(value);
            }
        }
        private Field<string> _name;
        #endregion fields

        private Product() : base() { }

        public static Product SelectOneById(string id)
        {
            return SelectOneById<Product>(id);
        }

        public static Product CreateWithOld(Product old, params (string name, object value)[] parameters)
        {
            return CreateWithOld<Product>(old, parameters);
        }

        public static Product Create(string name, string id = null)
        {
            Product product = new Product()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                _name = new Field<string>(name, nameof(Name))
            };

            return product;
        }
    }
}
