namespace Graph
{
    public class Node<T>
    {
        public Node() 
        {            
        }

        public Node(T data) : this(data, null) 
        {            
        }

        public Node(T data, NodeList<T> neighbors)
        {
            this.Value = data;
            this.Neighbors = neighbors;
        }

        public T Value {get; set;}

        protected NodeList<T> Neighbors {get; set;}
    }
}