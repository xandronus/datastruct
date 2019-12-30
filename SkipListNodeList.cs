namespace Graph
{
    /// <summary>
    /// Represents a collection of SkipListNodes.  This class differs from the base class - NodeList -
    /// in that it contains an internal method to increment or decrement the height of the SkipListNodeList. 
    /// Incrementing the height adds a new neighbor to the list, decrementing the height removes the
    /// top-most neighbor.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the SkipListNode instances that are contained
    /// within this SkipListNodeList.</typeparam>
    public class SkipListNodeList<T> : NodeList<T>
    {
        public SkipListNodeList(int height) : base(height) 
        {            
        }

        /// <summary>
        /// Increases the size of the SkipListNodeList by one, adding a default SkipListNode.
        /// </summary>
        internal void IncrementHeight()
        {
            // add a dummy entry
            base.Items.Add(default(Node<T>));
        }

        /// <summary>
        /// Decreases the size of the SkipListNodeList by one, removing the "top-most" SkipListNode.
        /// </summary>
        internal void DecrementHeight()
        {
            // delete the last entry
            base.Items.RemoveAt(base.Items.Count - 1);
        }
    }
}