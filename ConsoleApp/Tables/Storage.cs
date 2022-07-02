using System;
using Core;

namespace Tables
{
    internal class Storage : Table
    {
        #region fields
        public Store Store
        {
            get
            {
                return _store.Value;
            }
            set
            {
                _store.Set(value);
            }
        }
        private Field<Store> _store;

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

        private Storage() : base() { }

        public static Storage SelectOneById(string id)
        {
            return SelectOneById<Storage>(id);
        }

        public static Storage CreateWithOld(Storage old, params (string name, object value)[] parameters)
        {
            return CreateWithOld<Storage>(old, parameters);
        }

        public static Storage Create(Store store, string name, string id = null)
        {
            Storage storage = new Storage()
            {
                Id = id == null ? Guid.NewGuid() : Guid.Parse(id),
                _store = new Field<Store>(store, nameof(Store)),
                _name = new Field<string>(name, nameof(Name))
            };

            return storage;
        }
    }
}
