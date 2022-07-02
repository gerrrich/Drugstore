namespace Core
{
    public class Field<T>
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
    }
}
