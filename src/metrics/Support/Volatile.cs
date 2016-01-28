using System.Threading;

namespace metrics.Support
{
    /// <summary>
    /// Provides support for volatile operations around a typed value
    /// </summary>
    internal struct Volatile<T>
    {
        private object _value;

        private Volatile(T value) : this()
        {
            Set(value);
        }

        public void Set(T value)
        {
#if COREFX
            _value = value;
#else
            Thread.VolatileWrite(ref _value, value);
#endif
        }

        public T Get()
        {
#if COREFX
            return (T) _value;
#else
            return (T) Thread.VolatileRead(ref _value);
#endif
        }

        public static implicit operator Volatile<T>(T value)
        {
            return new Volatile<T>(value);
        }

        public static implicit operator T(Volatile<T> value)
        {
            return value.Get();
        }

        public override string ToString()
        {
            return Get().ToString();
        }
    }
}