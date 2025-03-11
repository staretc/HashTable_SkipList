namespace SkipListLib
{
    internal class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Right;
        public Node<TKey, TValue> Up;
        public Node<TKey, TValue> Down;
        public Node() : this(default, default) { }
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            Right = null;
            Up = null;
            Down = null;
        }
    }
}
