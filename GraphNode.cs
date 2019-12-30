using System.Collections.Generic;

namespace Graph
{
    public class GraphNode<T> : Node<T>
    {
        public GraphNode() : base() 
        {             
        }

        public GraphNode(T value) : base(value) 
        {             
        }
        
        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) 
        {             
        }

        new public NodeList<T> Neighbors {get; set;} = new NodeList<T>();

        public List<int> Costs {get; set;} = new List<int>();
    }
}