using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    /// <summary>
    /// Represents a graph.  A graph is an arbitrary collection of GraphNode instances.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the graph's nodes.</typeparam>
    public class Graph<T> : IEnumerable<T>
    {
        public NodeList<T> NodeSet {get; private set;}        // the set of nodes in the graph
        public IEnumerable<GraphNode<T>> GraphNodes => NodeSet.Select(i => i as GraphNode<T>);

        /// <summary>
        /// Returns the number of vertices in the graph.
        /// </summary>
        public int Count
        {
            get { return this.NodeSet.Count; }
        }

        public Graph() : this(null) 
        {            
        }

        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.NodeSet = new NodeList<T>();
            else
                this.NodeSet = nodeSet;
        }

        /// <summary>
        /// Adds a new GraphNode instance to the Graph
        /// </summary>
        /// <param name="node">The GraphNode instance to add.</param>
        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            this.NodeSet.Add(node);
        }

        /// <summary>
        /// Adds a new value to the graph.
        /// </summary>
        /// <param name="value">The value to add to the graph</param>
        public void AddNode(T value)
        {
            this.NodeSet.Add(new GraphNode<T>(value));
        }

        /// <summary>
        /// Adds a directed edge from a GraphNode with one value (from) to a GraphNode with another value (to).
        /// </summary>
        /// <param name="from">The value of the GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The value of the GraphNode to which the edge leads.</param>
        public void AddDirectedEdge(T from, T to)
        {
            this.AddDirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds a directed edge from one GraphNode (from) to another (to).
        /// </summary>
        /// <param name="from">The GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The GraphNode to which the edge leads.</param>
        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            this.AddDirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds a directed edge from one GraphNode (from) to another (to) with an associated cost.
        /// </summary>
        /// <param name="from">The GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The GraphNode to which the edge leads.</param>
        /// <param name="cost">The cost of the edge from "from" to "to".</param>
        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        /// <summary>
        /// Adds a directed edge from a GraphNode with one value (from) to a GraphNode with another value (to)
        /// with an associated cost.
        /// </summary>
        /// <param name="from">The value of the GraphNode from which the directed edge eminates.</param>
        /// <param name="to">The value of the GraphNode to which the edge leads.</param>
        /// <param name="cost">The cost of the edge from "from" to "to".</param>
        public void AddDirectedEdge(T from, T to, int cost)
        {
            ((GraphNode<T>) this.NodeSet.FindByValue(from)).Neighbors.Add(this.NodeSet.FindByValue(to));
            ((GraphNode<T>) this.NodeSet.FindByValue(from)).Costs.Add(cost);
        }

        /// <summary>
        /// Adds an undirected edge from a GraphNode with one value (from) to a GraphNode with another value (to).
        /// </summary>
        /// <param name="from">The value of one of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">The value of one of the GraphNodes that is joined by the edge.</param>
        public void AddUndirectedEdge(T from, T to)
        {
            this.AddUndirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds an undirected edge from one GraphNode to another.
        /// </summary>
        /// <param name="from">One of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">One of the GraphNodes that is joined by the edge.</param>
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            this.AddUndirectedEdge(from, to, 0);
        }

        /// <summary>
        /// Adds an undirected edge from one GraphNode to another with an associated cost.
        /// </summary>
        /// <param name="from">One of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">One of the GraphNodes that is joined by the edge.</param>
        /// <param name="cost">The cost of the undirected edge.</param>
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        /// <summary>
        /// Adds an undirected edge from a GraphNode with one value (from) to a GraphNode with another value (to)
        /// with an associated cost.
        /// </summary>
        /// <param name="from">The value of one of the GraphNodes that is joined by the edge.</param>
        /// <param name="to">The value of one of the GraphNodes that is joined by the edge.</param>
        /// <param name="cost">The cost of the undirected edge.</param>
        public void AddUndirectedEdge(T from, T to, int cost)
        {
            ((GraphNode<T>) this.NodeSet.FindByValue(from)).Neighbors.Add(this.NodeSet.FindByValue(to));
            ((GraphNode<T>) this.NodeSet.FindByValue(from)).Costs.Add(cost);

            ((GraphNode<T>) this.NodeSet.FindByValue(to)).Neighbors.Add(this.NodeSet.FindByValue(from));
            ((GraphNode<T>) this.NodeSet.FindByValue(to)).Costs.Add(cost);
        }

        /// <summary>
        /// Clears out the contents of the Graph.
        /// </summary>
        public void Clear()
        {
            this.NodeSet.Clear();
        }

        /// <summary>
        /// Returns a Boolean, indicating if a particular value exists within the graph.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns>True if the value exist in the graph; false otherwise.</returns>
        public bool Contains(T value)
        {
            return this.NodeSet.FindByValue(value) != null;
        }

        /// <summary>
        /// Attempts to remove a node from a graph.
        /// </summary>
        /// <param name="value">The value that is to be removed from the graph.</param>
        /// <returns>True if the corresponding node was found, and removed; false if the value was not
        /// present in the graph.</returns>
        /// <remarks>This method removes the GraphNode instance, and all edges leading to or from the
        /// GraphNode.</remarks>
        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>) this.NodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            this.NodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> graphNode in this.NodeSet)
            {
                int index = graphNode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    graphNode.Neighbors.RemoveAt(index);
                    graphNode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        /// <summary>
        /// Returns an enumerator that allows for iterating through the contents of the graph.
        public IEnumerator<T> GetEnumerator()
        {
            foreach (GraphNode<T> node in this.NodeSet)
                yield return node.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}