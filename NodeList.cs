using System.Collections.ObjectModel;
using System.Linq;

namespace Graph
{
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() 
        {             
        }

        public NodeList(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        public Node<T> FindByValue(T value)
        {
            return Items?.FirstOrDefault(i => i != null && i.Value.Equals(value));
        }
    }
}