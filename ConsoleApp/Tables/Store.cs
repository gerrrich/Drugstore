using System;
using Core;

namespace Tables
{
    internal class Store : Table
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

        public string Address
        {
            get
            {
                return _address.Value;
            }
            set
            {
                _address.Set(value);
            }
        }
        private Field<string> _address;

        public string Phone
        {
            get
            {
                return _phone.Value;
            }
            set
            {
                _phone.Set(value);
            }
        }
        private Field<string> _phone;
        #endregion fields

        private Store() : base() { }

        public static Store SelectOneById(string id)
        {
            return SelectOneById<Store>(id);
        }

        public static Store CreateWithOld(Store old, params (string name, object value)[] parameters)
        {
            return CreateWithOld<Store>(old, parameters);
        }

        public static Store Create(string name, string address, string phone, string id = null)
        {
            Store store = new Store()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                _name = new Field<string>(name, nameof(Name)),
                _address = new Field<string>(address, nameof(Address)),
                _phone = new Field<string>(phone, nameof(Phone))
            };

            return store;
        }
    }
}
