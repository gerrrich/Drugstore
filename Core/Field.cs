using System.Collections;
using System.Collections.Generic;

namespace Core
{
    public class Field<T> : IField
    {
        public T Value { get; private set; }

        public bool IsModified { get; private set; }

        public string Name { get; private set; }

        private Field() { }

        public Field(T value, string name)
        {
            // Field validation
            Value = value;
            Name = name;
        }

        public void Set(T value)
        {
            // Field validation
            Value = value;
        }

        public object GetValue()
        {
            return Value;
        }

        public string GetName()
        {
            return Name;
        }
    }

    public interface IField
    {
        string GetName();

        object GetValue();
    }
}
