namespace XcyUI.models
{
    public class XPare<T, V>
    {
        private XPare(T one,V two)
        {
            One = one;
            Two = two;
        }
        public T One { get; private set; }
        public V Two { get; private set; }

        public static XPare<T, V> Pare(T one, V two)
        {
            return new XPare<T, V>(one, two);
        }
    }
}
